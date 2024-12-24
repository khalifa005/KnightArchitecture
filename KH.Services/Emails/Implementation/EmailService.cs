using FluentEmail.Core;
using FluentEmail.Core.Models;
using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;
using Microsoft.Extensions.Options;
using KH.Services.Emails.Contracts;
using KH.Services.Emails.Helper;

public class EmailService : IEmailService
{
  private readonly IUserQueryService _userQueryService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly MailSettings _mailSettings;
  private readonly MailTemplatesSettings _mailTemplatesSettings;
  private IFluentEmailFactory _fluentEmail;
  private readonly ILogger<EmailService> _logger;
    private readonly IHostEnvironment _env;
  public EmailService(
    IFluentEmailFactory fluentEmail,
    IUserQueryService userQueryService,
    IUnitOfWork unitOfWork,
    IOptions<MailSettings> mailSettings,
    IOptions<MailTemplatesSettings> mailTemplatesSettings,
    IHostEnvironment env,
    ILogger<EmailService> loggerFactory)
  {
    _fluentEmail = fluentEmail;
    _mailSettings = mailSettings.Value;
    _mailTemplatesSettings = mailTemplatesSettings.Value;
    _logger = loggerFactory;
    _userQueryService = userQueryService;
    _unitOfWork = unitOfWork;
    _env = env;
  }

  public async Task<ApiResponse<string>> SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken, bool isResend = false)
  {
    var res = new ApiResponse<string>((int)HttpStatusCode.OK);
    List<MemoryStream> memoryStreams = new List<MemoryStream>();
    bool isSent = false;
    string failerReasons = "";

    try
    {
      _logger.LogDebug("SendEmailAsync started type {etype}", mailRequest.MailType.ToString());

      if (mailRequest == null || mailRequest.ModelId == 0 || mailRequest.Model == null)
        throw new ArgumentException("Invalid mail request parameters.");

      if (!_mailSettings.Disable)
      {
        string filePath = EmailHelper.GetEmailTemplatePath(mailRequest, _mailTemplatesSettings);
        var toRecipients = EmailHelper.SetEmailRecipients(mailRequest.ToEmail);
        var ccRecipients = EmailHelper.SetEmailRecipients(mailRequest.ToCCEmail);
        var attachments = EmailHelper.SetEmailAttachments(mailRequest.Attachments, memoryStreams);

        var sendingResult = await SendEmailByTypeAsync(mailRequest, filePath, toRecipients, ccRecipients, attachments, cancellationToken);
        if (!sendingResult.Successful)
          throw new ArgumentException(string.Join(',', sendingResult.ErrorMessages));

        isSent = true;
        res.Data = "Email sent successfully.";
        _logger.LogDebug($"send Email to ({string.Join(",", toRecipients.Select(o => o.EmailAddress))}) for model Id ({mailRequest.ModelId}) with Type ({mailRequest.MailType}) Succesded");
      }

      return res;
    }
    catch (Exception ex)
    {
      isSent = false;
      failerReasons = ex.Message;
      _logger.LogError($"send Email to {string.Join(",", mailRequest.ToEmail)} for user Id ({mailRequest.ModelId}) with Type {mailRequest.MailType} has error {ex.Message}");
      res.ErrorMessage = ex.Message;
      res.StatusCode = (int)HttpStatusCode.BadRequest;
      return res;
    }
    finally
    {
      // Ensure streams are disposed
      foreach (var stream in memoryStreams)
      {
        stream.Dispose();
      }

      if (!isResend)
      {
        var mailEntity = mailRequest.ToEntity();
        mailEntity.IsSent = isSent;
        mailEntity.FailReasons = failerReasons;
        await SaveEmailAsync(mailEntity, cancellationToken);
      }

    }
  }
  public async Task<ApiResponse<string>> SendMultipleEmailsAsync(List<MailRequest> mailRequests, CancellationToken cancellationToken)
  {
    var response = new ApiResponse<string>((int)HttpStatusCode.OK);
    var tasks = new List<Task>(); // A list to hold all email sending tasks

    try
    {
      // Parallelize sending each email in the list
      foreach (var mailRequest in mailRequests)
      {
        tasks.Add(SendEmailAsync(mailRequest, cancellationToken));
      }

      // Await all email sending tasks
      await Task.WhenAll(tasks);

      response.Data = "All emails sent successfully.";
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error in sending multiple emails: {ex.Message}");
      response.StatusCode = (int)HttpStatusCode.InternalServerError;
      response.ErrorMessage = "Error occurred while sending multiple emails.";
    }

    return response;
  }
  public async Task<ApiResponse<string>> ResendEmailAsync(long emailTrackerId, CancellationToken cancellationToken)
  {
    var response = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<EmailTracker>();
      var emailTracker = await repository.GetAsync(emailTrackerId,tracking:true, cancellationToken: cancellationToken);

      if (emailTracker == null)
      {
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        response.ErrorMessage = "Email not found.";
        return response;
      }

      // Prepare the resend using the emailTracker details
      var mailRequest = new MailRequest
      {
        ToEmail = new List<string> { emailTracker.ToEmail },
        Subject = emailTracker.Subject,
        Body = emailTracker.Body,
        Attachments = null // Handle attachments if necessary
      };

      // Call SendEmailAsync but prevent it from creating a new EmailTracker
      var isResend = true; 
      var sendResult = await SendEmailAsync(mailRequest, cancellationToken, isResend);
      if (sendResult.StatusCode == (int)HttpStatusCode.OK)
      {
        emailTracker.IsSent = true;
        emailTracker.FailReasons = string.Empty;
      }
      else
      {
        emailTracker.IsSent = false;
        emailTracker.FailReasons = sendResult.ErrorMessage;
      }

      await _unitOfWork.CommitAsync(cancellationToken);

      response.Data = "Email resent successfully.";
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error resending email: {ex.Message}");
      response.StatusCode = (int)HttpStatusCode.InternalServerError;
      response.ErrorMessage = "Error occurred while resending email.";
    }

    return response;
  }
  public async Task<ApiResponse<string>> ResendRangeOfScheduledEmailsAsync(int batchSize, CancellationToken cancellationToken)
  {
    var res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<EmailTracker>();
      int currentPage = 1;
      bool hasMoreEmails = true;

      // Process emails in batches
      while (hasMoreEmails)
      {
        // Get a batch of unsent emails
        var missedEmails = await repository.GetPagedWithProjectionAsync<EmailTracker>(
            pageNumber: currentPage,
            pageSize: batchSize,
            filterExpression: e => !e.IsSent, // Unsent emails - more specific depend on schedule datetime
            projectionExpression: e => e, // Direct projection
            orderBy: query => query.OrderBy(e => e.Id), // Order by ID for consistency
            tracking: true, // Enable tracking so we can update status
            cancellationToken: cancellationToken
        );

        if (!missedEmails.Items.Any())
        {
          hasMoreEmails = false;
          break;
        }

        // Send emails in parallel (we limit concurrency to avoid overloading the system)
        var emailTasks = missedEmails.Items.Select(email => Task.Run(async () =>
        {
          try
          {
            var mailRequest = new MailRequest
            {
              ToEmail = email.ToEmail.Split(',').ToList(),
              ToCCEmail = email.ToCCEmail?.Split(',').ToList(),
              Subject = email.Subject,
              Body = email.Body,
              MailType = EmailHelper.GetMailTypeEnum(email.MailType),
              ModelId = email.ModelId,
              Model = email.Model,
              Attachments = null // Assuming attachments are not stored in EmailTracker
            };

            // Send the email
            var sendResult = await SendEmailAsync(mailRequest, cancellationToken, isResend:true);

            // Update the email tracker after sending
            if (sendResult.StatusCode == (int)HttpStatusCode.OK)
            {
              email.IsSent = true;
              email.FailReasons = string.Empty;
            }
            else
            {
              email.FailReasons = sendResult.ErrorMessage;
            }
          }
          catch (Exception ex)
          {
            _logger.LogError($"Failed to resend email with ID {email.Id}: {ex.Message}");
            email.FailReasons = ex.Message;
          }
        }));

        // Wait for all tasks in this batch to complete
        await Task.WhenAll(emailTasks);

        // Commit updates to all emails in the batch
        await _unitOfWork.CommitAsync(cancellationToken);

        // Move to the next page
        currentPage++;
      }

      res.Data = "Resend process for missed emails completed.";
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error in ResendRangeOfMissedEmailsAsync: {ex.Message}");
      res.ErrorMessage = ex.Message;
      res.StatusCode = (int)HttpStatusCode.InternalServerError;
    }

    return res;
  }
  public async Task<ApiResponse<string>> ScheduleEmailAsync(MailRequest mailRequest, DateTime? scheduledTime, CancellationToken cancellationToken)
  {
    var response = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      _logger.LogDebug("Scheduling email for later time.");

      if (mailRequest == null || mailRequest.ModelId == 0 || mailRequest.Model == null)
        throw new ArgumentException("Invalid mail request parameters.");

      var emailEntity = mailRequest.ToEntity();
      emailEntity.IsSent = false; // Mark as unsent
      emailEntity.ScheduleSendDate = scheduledTime; // Set the scheduled time for sending

      // Save to database but do not send
      await SaveEmailAsync(emailEntity, cancellationToken);

      response.Data = "Email scheduled successfully.";
      _logger.LogDebug($"Email scheduled to send at {scheduledTime} for recipient(s) {string.Join(",", mailRequest.ToEmail)}");

      return response;
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error scheduling email: {ex.Message}");
      response.StatusCode = (int)HttpStatusCode.InternalServerError;
      response.ErrorMessage = "Error occurred while scheduling email.";
      return response;
    }
  }

  private async Task<ApiResponse<string>> SaveEmailAsync(EmailTracker request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    bool isModelExists = Enum.IsDefined(typeof(ModelEnum), request.Model);

    try
    {

      var repository = _unitOfWork.Repository<EmailTracker>();

      await repository.AddAsync(request, cancellationToken: cancellationToken);
      await _unitOfWork.CommitAsync(cancellationToken);

      res.Data = request.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      return ex.HandleException(res, _env, _logger);
    }
  }
  private async Task<SendResponse> SendEmailByTypeAsync(MailRequest mailRequest, string filePath, List<Address> toRecipients, List<Address> ccRecipients, List<FluentEmail.Core.Models.Attachment> attachments, CancellationToken cancellationToken)
  {
    // Filter out empty or whitespace-only addresses from recipient lists
    toRecipients = toRecipients.Where(r => !string.IsNullOrWhiteSpace(r.EmailAddress)).ToList();
    ccRecipients = ccRecipients.Where(r => !string.IsNullOrWhiteSpace(r.EmailAddress)).ToList();

    switch (mailRequest.MailType)
    {
      case MailTypeEnum.WelcomeTemplate:
        var targetUser = await _userQueryService.GetAsync(mailRequest.ModelId, cancellationToken);
        if (targetUser.Data is not object)
          throw new Exception("No user defined with this ID for this email type");

        var emailTemplateResult = _fluentEmail.Create()
            .To(toRecipients)
            .CC(ccRecipients)
            .Attach(attachments)
            .Subject(mailRequest.Subject)
            .UsingTemplateFromFile(filePath, targetUser.Data);

        var sentResult = await emailTemplateResult.SendAsync(cancellationToken);
        return sentResult;

      default:
        var defaultSentResult =  await _fluentEmail.Create()
            .To(toRecipients)
            .CC(ccRecipients)
            .Subject(mailRequest.Subject)
            .Body(mailRequest.Body, true)
            .Attach(attachments)
            .SendAsync(cancellationToken);
        return defaultSentResult;

    }
  }

}
