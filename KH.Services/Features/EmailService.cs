using FluentEmail.Core;
using FluentEmail.Core.Models;
using KH.Domain.Enums;
using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;
using KH.Helper.Settings;
using KH.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

public class EmailService : IEmailService
{
  private readonly IUserService _userService;
  private readonly MailSettings _mailSettings;
  private readonly MailTemplatesSettings _mailTemplatesSettings;
  private IFluentEmailFactory _fluentEmail;
  private readonly ILogger<EmailService> _loggerFactory;
  public EmailService(
    IFluentEmailFactory fluentEmail,
    IUserService userService,
    IOptions<MailSettings> mailSettings,
    IOptions<MailTemplatesSettings> mailTemplatesSettings,
    ILogger<EmailService> loggerFactory)
  {
    _fluentEmail = fluentEmail;
    _mailSettings = mailSettings.Value;
    _mailTemplatesSettings = mailTemplatesSettings.Value;
    _loggerFactory = loggerFactory;
    _userService = userService;
  }

  public async Task<ApiResponse<object>> SendEmailAsync(MailRequest mailRequest)
  {
    var res = new ApiResponse<object>((int)HttpStatusCode.OK);
    var test = (MailTypeEnum)System.Enum.GetValues(typeof(MailTypeEnum)).GetValue(mailRequest.MailTypeId);

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

        ////--To Email
        var toRecipients = SetEmailRecipients(mailRequest.ToEmail);
        _loggerFactory.LogInformation($"TO Users : After set List ({string.Join(",", toRecipients.Select(o => o.EmailAddress))})");
        //-- TO CC
        var ccRecipients = SetEmailRecipients(mailRequest.ToCCEmail);
        _loggerFactory.LogInformation($"CC Users : After set List ({string.Join(",", ccRecipients.Select(o => o.EmailAddress))})");
        ////-- Attachments
        //var attachments = SetEmailAttachments(mailRequest.Attachments);

        switch (mailRequest.MailType)
        {
          case MailTypeEnum.WelcomeTemplate:
            {
              //--Fill Model
              var targetUser = await _userService.GetAsync(mailRequest.UserId);
              if (targetUser.Data is not object)
                throw new Exception("No user defiend with this id for this email type");

              var userInfo = targetUser.Data;

              //_fluentEmail
              //    .Create().To(toRecipients)
              //                  .CC(ccRecipients)
              //                  .Subject(mailRequest.Subject)
              //                  .Attach(attachments)
              //                  .UsingTemplateFromFile(filePath, userInfo)
              //                  .Send();


              var emailTemplateResult = _fluentEmail.Create().To(toRecipients)
                                .CC(ccRecipients)
                                .Subject(mailRequest.Subject)
                                .UsingTemplateFromFile(filePath, userInfo);

              await emailTemplateResult.SendAsync();

              break;
            }
          case MailTypeEnum.TicketEscalation:
            {
              _loggerFactory.LogInformation(" prepare the query of the ticket Email");
              break;
            }
          default:
            {
              await _fluentEmail.Create().To(toRecipients)
                                .CC(ccRecipients)
                                .Subject(mailRequest.Subject)
                                .Body(mailRequest.Body, true)
                                .SendAsync();
              break;
            }
        }

        _loggerFactory.LogInformation($"send Email to ({string.Join(",", toRecipients.Select(o => o.EmailAddress))}) for model Id ({mailRequest.UserId}) with Type ({mailRequest.MailType}) Succesded");
      }
      return res;
    }
    catch (Exception ex)
    {
      _loggerFactory.LogError($"send Email to {string.Join(",", mailRequest.ToEmail)} for user Id ({mailRequest.UserId}) with Type {mailRequest.MailType} has error {ex.Message}");
      res.ErrorMessage = ex.Message;
      res.StatusCode = (int)HttpStatusCode.BadRequest;
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
  private static List<Attachment> SetEmailAttachments(List<IFormFile>? attachments)
  {
    var emailAttachments = new List<Attachment>();
    if (attachments != null)
    {
      foreach (var file in attachments)
      {
        if (file.Length > 0)
        {
          using (var ms = new MemoryStream())
          {
            file.CopyTo(ms);
            emailAttachments.Add(new Attachment { Filename = file.FileName, Data = ms, ContentType = file.ContentType });
          }
        }
      }
    }

    return emailAttachments;
  }
  //public async Task SendOrderConfirmationAsync(OrderConfirmationModel model)
  //{
  //  string templatePath = $"{Directory.GetCurrentDirectory()}\\Templates\\Emails\\OrderConfirmation.cshtml";


  //  var email = _fluentEmail
  //      .To("mahmudkhalifa1@gmail.com", model.UserName)
  //      .Subject("Your Order Confirmation")
  //      .UsingTemplateFromFile(templatePath, model);

  //  // Attach invoice if necessary
  //  if (!string.IsNullOrEmpty(model.OrderId))
  //  {
  //    email.Attach(new FluentEmail.Core.Models.Attachment
  //    {
  //      //Data = new MemoryStream(File.ReadAllBytes($"Attachments/Invoice_{model.OrderId}.pdf")),
  //      Data = new MemoryStream(File.ReadAllBytes($"C:\\Application_Upload\\User_8\\09-25-2024-1-19-keyboard-shortcuts.pdf")),
  //      ContentType = "application/pdf",
  //      Filename = $"Invoice_{model.OrderId}.pdf"
  //    });
  //  }

  //  await email.SendAsync();
  //}

}
