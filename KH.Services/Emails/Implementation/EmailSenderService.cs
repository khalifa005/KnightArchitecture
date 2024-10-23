//using FluentEmail.Core;
//using FluentEmail.Core.Models;
//using KH.Dto.Models.EmailDto.Request;
//using Microsoft.Extensions.Options;
//using KH.Services.Emails.Contracts;

//public class EmailSenderService : IEmailSenderService
//{
//  private readonly IFluentEmailFactory _fluentEmail;
//  private readonly MailSettings _mailSettings;
//  private readonly MailTemplatesSettings _mailTemplatesSettings;
//  private readonly IUserQueryService _userQueryService;
//  private readonly ILogger<EmailSenderService> _logger;

//  public EmailSenderService(
//      IFluentEmailFactory fluentEmail,
//      IOptions<MailSettings> mailSettings,
//      IOptions<MailTemplatesSettings> mailTemplatesSettings,
//      IUserQueryService userQueryService,
//      ILogger<EmailSenderService> logger)
//  {
//    _fluentEmail = fluentEmail;
//    _mailSettings = mailSettings.Value;
//    _mailTemplatesSettings = mailTemplatesSettings.Value;
//    _userQueryService = userQueryService;
//    _logger = logger;
//  }

//  public async Task<ApiResponse<string>> SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken)
//  {
//    var res = new ApiResponse<string>((int)HttpStatusCode.OK);
//    List<MemoryStream> memoryStreams = new List<MemoryStream>();
//    bool isSent = false;
//    string failerReasons = "";

//    try
//    {
//      _logger.LogInformation("SendEmailAsync started for email type {etype}", mailRequest.MailType.ToString());

//      if (mailRequest == null || mailRequest.ModelId == 0 || mailRequest.Model == null)
//        throw new ArgumentException("Invalid mail request parameters.");

//      if (!_mailSettings.Disable)
//      {
//        string filePath = GetEmailTemplatePath(mailRequest);
//        var toRecipients = SetEmailRecipients(mailRequest.ToEmail);
//        var ccRecipients = SetEmailRecipients(mailRequest.ToCCEmail);
//        var attachments = SetEmailAttachments(mailRequest.Attachments, memoryStreams);

//        await SendEmailByTypeAsync(mailRequest, filePath, toRecipients, ccRecipients, attachments, cancellationToken);
//        isSent = true;
//        res.Data = "Email sent successfully.";
//      }

//      return res;
//    }
//    catch (Exception ex)
//    {
//      isSent = false;
//      failerReasons = ex.Message;
//      _logger.LogError($"Error sending email: {ex.Message}");
//      res.ErrorMessage = ex.Message;
//      res.StatusCode = (int)HttpStatusCode.BadRequest;
//      return res;
//    }
//    finally
//    {
//      foreach (var stream in memoryStreams)
//      {
//        stream.Dispose();
//      }
//    }
//  }

//  public async Task<ApiResponse<string>> SendMultipleEmailsAsync(List<MailRequest> mailRequests, CancellationToken cancellationToken)
//  {
//    var tasks = mailRequests.Select(mailRequest => SendEmailAsync(mailRequest, cancellationToken));
//    await Task.WhenAll(tasks);
//    return new ApiResponse<string>((int)HttpStatusCode.OK) { Data = "All emails sent successfully." };
//  }

//  private async Task SendEmailByTypeAsync(MailRequest mailRequest, string filePath, List<Address> toRecipients, List<Address> ccRecipients, List<FluentEmail.Core.Models.Attachment> attachments, CancellationToken cancellationToken)
//  {
//    switch (mailRequest.MailType)
//    {
//      case MailTypeEnum.WelcomeTemplate:
//        var targetUser = await _userQueryService.GetAsync(mailRequest.ModelId, cancellationToken);
//        if (targetUser.Data is not object)
//          throw new Exception("No user defined with this ID for this email type");

//        var emailTemplateResult = _fluentEmail.Create()
//            .To(toRecipients)
//            .CC(ccRecipients)
//            .Attach(attachments)
//            .Subject(mailRequest.Subject)
//            .UsingTemplateFromFile(filePath, targetUser.Data);

//        await emailTemplateResult.SendAsync(cancellationToken);
//        break;

//      default:
//        await _fluentEmail.Create()
//            .To(toRecipients)
//            .CC(ccRecipients)
//            .Subject(mailRequest.Subject)
//            .Body(mailRequest.Body, true)
//            .Attach(attachments)
//            .SendAsync(cancellationToken);
//        break;
//    }
//  }

//  private string GetEmailTemplatePath(MailRequest mailRequest)
//  {
//    if (mailRequest.MailType != MailTypeEnum.Default)
//    {
//      var templatePath = _mailTemplatesSettings.Types.FirstOrDefault(o => o.MailType == mailRequest.MailType)?.TemplatePath;
//      if (string.IsNullOrEmpty(templatePath))
//        throw new Exception("No template path defined for this email type");
//      return $"{Directory.GetCurrentDirectory()}\\Templates\\Emails\\{templatePath}";
//    }
//    return string.Empty;
//  }

//  private List<Address> SetEmailRecipients(List<string?>? emailRecipients)
//  {
//    return emailRecipients?.Select(toMember => new Address { EmailAddress = toMember }).ToList() ?? new List<Address>();
//  }

//  private List<FluentEmail.Core.Models.Attachment> SetEmailAttachments(List<IFormFile>? attachments, List<MemoryStream> memoryStreams)
//  {
//    var emailAttachments = new List<FluentEmail.Core.Models.Attachment>();
//    if (attachments != null)
//    {
//      foreach (var file in attachments)
//      {
//        var ms = new MemoryStream();
//        file.CopyTo(ms);
//        ms.Flush();
//        ms.Position = 0;

//        emailAttachments.Add(new FluentEmail.Core.Models.Attachment { Filename = file.FileName, Data = ms, ContentType = file.ContentType });
//        memoryStreams.Add(ms);
//      }
//    }
//    return emailAttachments;
//  }
//}
