using FluentEmail.Core;
using FluentEmail.Core.Models;
using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;
using Microsoft.Extensions.Options;
using FluentEmail.Core.Models;
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

  public async Task<ApiResponse<object>> SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken)
  {
    var res = new ApiResponse<object>((int)HttpStatusCode.OK);
    List<MemoryStream> memoryStreams = new List<MemoryStream>();
    bool isSent = false;
    string failerReasons = "";

    try
    {
      _logger.LogInformation("SendEmailAsync started type {etype}", mailRequest.MailType.ToString());

      if (mailRequest == null)
        throw new Exception("Invalid Parameter");


      if (mailRequest.Model is null || mailRequest.ModelId == 0)
        throw new Exception("Invalid Parameter");


      if (!_mailSettings.Disable)
      {
        string filePath = "";

        if (mailRequest.MailType != MailTypeEnum.Default)
        {
          var templatePath = _mailTemplatesSettings.Types.Where(o => o.MailType == mailRequest?.MailType).FirstOrDefault()?.TemplatePath ?? "";
          if (templatePath.IsNullOrEmpty())
            throw new Exception("No template path defiend for this email type");

          filePath = $"{Directory.GetCurrentDirectory()}\\Templates\\Emails\\{templatePath}";
          //check if the filePath exist
        }
        else if (mailRequest.MailType == MailTypeEnum.Default && mailRequest.Body.IsNullOrEmpty())
          throw new Exception("No body defiend for this email type");

        var toRecipients = SetEmailRecipients(mailRequest.ToEmail);
        var ccRecipients = SetEmailRecipients(mailRequest.ToCCEmail);
        var attachments = SetEmailAttachments(mailRequest.Attachments, memoryStreams);

        switch (mailRequest.MailType)
        {
          case MailTypeEnum.WelcomeTemplate:
            {
              var targetUser = await _userQueryService.GetAsync(mailRequest.ModelId, cancellationToken);
              if (targetUser.Data is not object)
                throw new Exception("No user defiend with this id for this email type");

              var userInfo = targetUser.Data;
              userInfo.PrefaredLanguageKey = mailRequest.PrefaredLanguageKey;


              var emailTemplateResult = _fluentEmail.Create().To(toRecipients)
                                .CC(ccRecipients)
                                .Attach(attachments)
                                .Subject(mailRequest.Subject)
                                .UsingTemplateFromFile(filePath, userInfo);

              await emailTemplateResult.SendAsync(cancellationToken);
              isSent = true;
              break;
            }
          case MailTypeEnum.TicketEscalation:
            {
              _logger.LogInformation(" prepare the query of the ticket Email");
              isSent = true;
              break;
            }
          default:
            {
              await _fluentEmail.Create().To(toRecipients)
                                .CC(ccRecipients)
                                .Subject(mailRequest.Subject)
                                .Body(mailRequest.Body, true)
                                .SendAsync(cancellationToken);
              isSent = true;
              break;
            }
        }

        _logger.LogInformation($"send Email to ({string.Join(",", toRecipients.Select(o => o.EmailAddress))}) for model Id ({mailRequest.ModelId}) with Type ({mailRequest.MailType}) Succesded");
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

      var mailEntity = mailRequest.ToEntity();
      mailEntity.IsSent = isSent;
      mailEntity.FailReasons = failerReasons;

      await AddAsync(mailEntity, cancellationToken);
    }
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
}
