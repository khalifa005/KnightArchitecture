using FluentEmail.Core;
using FluentEmail.Core.Models;
using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;
using Microsoft.Extensions.Options;
using KH.Services.Emails.Contracts;

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
        ///new code
        string filePath = GetEmailTemplatePath(mailRequest);
        var toRecipients = SetEmailRecipients(mailRequest.ToEmail);
        var ccRecipients = SetEmailRecipients(mailRequest.ToCCEmail);
        var attachments = SetEmailAttachments(mailRequest.Attachments, memoryStreams);

        await SendEmailByTypeAsync(mailRequest, filePath, toRecipients, ccRecipients, attachments, cancellationToken);
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
        await AddAsync(mailEntity, cancellationToken);
      }

    }
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
      var isResend = true; // Pass this flag to avoid creating a new EmailTracker
      await SendEmailAsync(mailRequest, cancellationToken, isResend);

      // Update the existing tracker entry to reflect that the email was resent
      emailTracker.IsSent = true;

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
  public async Task<ApiResponse<string>> ResendRangeOfMissedEmailsAsync(int batchSize, CancellationToken cancellationToken)
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
            filterExpression: e => !e.IsSent, // Unsent emails
            projectionExpression: e => e, // Direct projection
            orderBy: query => query.OrderBy(e => e.Id), // Order by ID for consistency
            tracking: true, // Enable tracking so we can update status
            cancellationToken: cancellationToken
        );

        if (!missedEmails.Any())
        {
          hasMoreEmails = false;
          break;
        }

        // Send emails in parallel (we limit concurrency to avoid overloading the system)
        var emailTasks = missedEmails.Select(email => Task.Run(async () =>
        {
          try
          {
            var mailRequest = new MailRequest
            {
              ToEmail = email.ToEmail.Split(',').ToList(),
              ToCCEmail = email.ToCCEmail?.Split(',').ToList(),
              Subject = email.Subject,
              Body = email.Body,
              MailType = GetMailTypeEnum(email.MailType),
              ModelId = email.ModelId,
              Attachments = null // Assuming attachments are not stored in EmailTracker
            };

            // Send the email
            var sendResult = await SendEmailAsync(mailRequest, cancellationToken);

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
  public async Task<ApiResponse<EmailTrackerResponse>> GetAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<EmailTrackerResponse>? res = new ApiResponse<EmailTrackerResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<EmailTracker>();

    //light user query to make sure the user exist
    var entityFromDB = await repository.GetAsync(id,cancellationToken:cancellationToken);

    if (entityFromDB == null)
    {
      res.StatusCode = (int)StatusCodes.Status400BadRequest;
      res.ErrorMessage = "invalid";
    }

    EmailTrackerResponse entityResponse = new EmailTrackerResponse(entityFromDB);

    res.Data = entityResponse;
    return res;
  }
  public async Task<ApiResponse<PagedResponse<EmailTrackerResponse>>> GetListAsync(MailRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<PagedResponse<EmailTrackerResponse>> apiResponse = new ApiResponse<PagedResponse<EmailTrackerResponse>>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<EmailTracker>();

    var pagedEntities = await repository.GetPagedWithProjectionAsync<EmailTrackerResponse>(
    pageNumber: request.PageIndex,
    pageSize: request.PageSize,
    filterExpression: u =>
    u.IsSent == request.IsSent
    && u.ModelId == request.ModelId
    && u.Model == request.Model, // Filter by

    projectionExpression: u => new EmailTrackerResponse(u),
    orderBy: query => query.OrderBy(u => u.Id),  // Sort by Id
    tracking: false,  // Disable tracking for read-only queries
    cancellationToken: cancellationToken
);

    var entitiesResponses = pagedEntities.Select(x => x).ToList();

    var pagedResponse = new PagedResponse<EmailTrackerResponse>(
      entitiesResponses,
       pagedEntities.CurrentPage,
       pagedEntities.TotalPages,
       pagedEntities.PageSize,
       pagedEntities.TotalCount);

    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
  private async Task<ApiResponse<string>> AddAsync(EmailTracker request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    bool isModelExists = Enum.IsDefined(typeof(ModelEnum), request.Model);

    await _unitOfWork.BeginTransactionAsync(cancellationToken);

    try
    {


      var repository = _unitOfWork.Repository<EmailTracker>();

      await repository.AddAsync(request, cancellationToken: cancellationToken);
      await _unitOfWork.CommitAsync(cancellationToken);

      await _unitOfWork.CommitTransactionAsync(cancellationToken);

      res.Data = request.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken);
      return ex.HandleException(res, _env, _logger);
    }
  }
  private static List<Address> SetEmailRecipients(List<string?>? emailRecipients)
  {
    var toRecipients = new List<Address>();
    if (emailRecipients != null)
    {
      foreach (var toMember in emailRecipients)
      {
        toRecipients.Add(new Address { EmailAddress = toMember });
      }
    }


    return toRecipients;
  }
  private static List<FluentEmail.Core.Models.Attachment> SetEmailAttachments(List<IFormFile>? attachments, List<MemoryStream> memoryStreams)
  {
    var emailAttachments = new List<FluentEmail.Core.Models.Attachment>();

    if (attachments != null)
    {
      foreach (var file in attachments)
      {
        if (file.Length > 0)
        {
          var ms = new MemoryStream();
          file.CopyTo(ms);

          // Ensure the stream is flushed and ready
          ms.Flush();
          ms.Position = 0; // Reset the position to ensure it's ready for reading

          // Add to the attachments
          emailAttachments.Add(new FluentEmail.Core.Models.Attachment
          {
            Filename = file.FileName,
            Data = ms,
            ContentType = file.ContentType
          });

          // Track the memory stream for disposal later
          memoryStreams.Add(ms);
        }
      }
    }

    return emailAttachments;
  }
  private static string GetContentType(string path)
  {
    var ext = Path.GetExtension(path).ToLowerInvariant();
    return ext switch
    {
      ".pdf" => "application/pdf",
      ".jpg" => "image/jpeg",
      ".png" => "image/png",
      ".doc" => "application/msword",
      ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
      _ => "application/octet-stream",
    };
  }

  private MailTypeEnum GetMailTypeEnum(string mailType)
  {
    return (MailTypeEnum)Enum.Parse(typeof(MailTypeEnum), mailType, true);
  }

  private string GetEmailTemplatePath(MailRequest mailRequest)
  {
    if (mailRequest.MailType != MailTypeEnum.Default)
    {
      var templatePath = _mailTemplatesSettings.Types.FirstOrDefault(o => o.MailType == mailRequest.MailType)?.TemplatePath;
      if (string.IsNullOrEmpty(templatePath))
        throw new Exception("No template path defined for this email type");
      return $"{Directory.GetCurrentDirectory()}\\Templates\\Emails\\{templatePath}";
    }
    return string.Empty;
  }

  private async Task SendEmailByTypeAsync(MailRequest mailRequest, string filePath, List<Address> toRecipients, List<Address> ccRecipients, List<FluentEmail.Core.Models.Attachment> attachments, CancellationToken cancellationToken)
  {
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

        await emailTemplateResult.SendAsync(cancellationToken);
        break;

      default:
        await _fluentEmail.Create()
            .To(toRecipients)
            .CC(ccRecipients)
            .Subject(mailRequest.Subject)
            .Body(mailRequest.Body, true)
            .Attach(attachments)
            .SendAsync(cancellationToken);
        break;
    }
  }

}
