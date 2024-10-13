using FluentEmail.Core;
using FluentEmail.Core.Models;
using KH.BuildingBlocks.Apis.Enums;
using KH.BuildingBlocks.Apis.Responses;
using KH.BuildingBlocks.Settings;
using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class EmailService : IEmailService
{
  private readonly IUserService _userService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly MailSettings _mailSettings;
  private readonly MailTemplatesSettings _mailTemplatesSettings;
  private IFluentEmailFactory _fluentEmail;
  private readonly ILogger<EmailService> _loggerFactory;
  public EmailService(
    IFluentEmailFactory fluentEmail,
    IUserService userService,
    IUnitOfWork unitOfWork,
    IOptions<MailSettings> mailSettings,
    IOptions<MailTemplatesSettings> mailTemplatesSettings,
    ILogger<EmailService> loggerFactory)
  {
    _fluentEmail = fluentEmail;
    _mailSettings = mailSettings.Value;
    _mailTemplatesSettings = mailTemplatesSettings.Value;
    _loggerFactory = loggerFactory;
    _userService = userService;
    _unitOfWork = unitOfWork;
  }

  public async Task<ApiResponse<object>> SendEmailAsync(MailRequest mailRequest)
  {
    var res = new ApiResponse<object>((int)HttpStatusCode.OK);
    List<MemoryStream> memoryStreams = new List<MemoryStream>();
    bool isSent = false;
    string failerReasons = "";

    try
    {
      _loggerFactory.LogInformation("SendEmailAsync started type {etype}", mailRequest.MailType.ToString());

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
              var targetUser = await _userService.GetAsync(mailRequest.ModelId);
              if (targetUser.Data is not object)
                throw new Exception("No user defiend with this id for this email type");

              var userInfo = targetUser.Data;

              var emailTemplateResult = _fluentEmail.Create().To(toRecipients)
                                .CC(ccRecipients)
                                .Attach(attachments)
                                .Subject(mailRequest.Subject)
                                .UsingTemplateFromFile(filePath, userInfo);

              await emailTemplateResult.SendAsync();
              isSent = true;
              break;
            }
          case MailTypeEnum.TicketEscalation:
            {
              _loggerFactory.LogInformation(" prepare the query of the ticket Email");
              isSent = true;
              break;
            }
          default:
            {
              await _fluentEmail.Create().To(toRecipients)
                                .CC(ccRecipients)
                                .Subject(mailRequest.Subject)
                                .Body(mailRequest.Body, true)
                                .SendAsync();
              isSent = true;
              break;
            }
        }

        _loggerFactory.LogInformation($"send Email to ({string.Join(",", toRecipients.Select(o => o.EmailAddress))}) for model Id ({mailRequest.ModelId}) with Type ({mailRequest.MailType}) Succesded");
      }
      return res;
    }
    catch (Exception ex)
    {
      isSent = false;
      failerReasons = ex.Message;
      _loggerFactory.LogError($"send Email to {string.Join(",", mailRequest.ToEmail)} for user Id ({mailRequest.ModelId}) with Type {mailRequest.MailType} has error {ex.Message}");
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

      await AddAsync(mailEntity);
    }
  }

  public async Task<ApiResponse<EmailTrackerResponse>> GetAsync(long id)
  {
    ApiResponse<EmailTrackerResponse>? res = new ApiResponse<EmailTrackerResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<EmailTracker>();

    //light user query to make sure the user exist
    var entityFromDB = await repository.GetAsync(id);

    if (entityFromDB == null)
    {
      res.StatusCode = (int)StatusCodes.Status400BadRequest;
      res.ErrorMessage = "invalid";
    }

    EmailTrackerResponse entityResponse = new EmailTrackerResponse(entityFromDB);

    res.Data = entityResponse;
    return res;
  }

  public async Task<ApiResponse<PagedResponse<EmailTrackerResponse>>> GetListAsync(MailRequest request)
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
    tracking: false  // Disable tracking for read-only queries
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


  private async Task<ApiResponse<string>> AddAsync(EmailTracker request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    bool isModelExists = Enum.IsDefined(typeof(ModelEnum), request.Model);

    await _unitOfWork.BeginTransactionAsync();

    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      //all validation should be in fluent validation side
      if (request.ToCCEmail.Length <= 0)
        throw new Exception("Invalid email");

      if (request.ModelId == null)
        throw new Exception("Invalid Parameter");


      var repository = _unitOfWork.Repository<EmailTracker>();

      await repository.AddAsync(request);
      await _unitOfWork.CommitAsync();

      await _unitOfWork.CommitTransactionAsync();

      res.Data = request.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync();

      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
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
  private static List<Attachment> SetEmailAttachments(List<IFormFile>? attachments, List<MemoryStream> memoryStreams)
  {
    var emailAttachments = new List<Attachment>();

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
          emailAttachments.Add(new Attachment
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
  private static List<Attachment> SetEmailAttachmentsXX(List<IFormFile>? attachments, List<string>? filePaths, List<MemoryStream> memoryStreams)
  {
    var emailAttachments = new List<Attachment>();

    // Handle attachments from IFormFile
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
          emailAttachments.Add(new Attachment
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

    // Handle file attachments directly from the disk using file paths
    if (filePaths != null)
    {
      foreach (var filePath in filePaths)
      {
        if (File.Exists(filePath))
        {
          var ms = new MemoryStream(File.ReadAllBytes(filePath));

          emailAttachments.Add(new Attachment
          {
            Filename = Path.GetFileName(filePath),
            Data = ms,
            ContentType = GetContentType(filePath) // Auto-detect content type based on file extension
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
