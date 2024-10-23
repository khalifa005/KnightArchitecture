using FluentEmail.Core.Models;
using KH.Dto.Models.EmailDto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Services.Emails.Helper;
public static class EmailHelper
{
  public static List<Address> SetEmailRecipients(List<string?>? emailRecipients)
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

  public static List<FluentEmail.Core.Models.Attachment> SetEmailAttachments(List<IFormFile>? attachments, List<MemoryStream> memoryStreams)
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
          ms.Flush();
          ms.Position = 0; // Reset the position to ensure it's ready for reading

          emailAttachments.Add(new FluentEmail.Core.Models.Attachment
          {
            Filename = file.FileName,
            Data = ms,
            ContentType = file.ContentType
          });

          memoryStreams.Add(ms);
        }
      }
    }

    return emailAttachments;
  }

  public static string GetContentType(string path)
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

  public static string GetEmailTemplatePath(MailRequest mailRequest, MailTemplatesSettings mailTemplatesSettings)
  {
    if (mailRequest.MailType != MailTypeEnum.Default)
    {
      var templatePath = mailTemplatesSettings.Types.FirstOrDefault(o => o.MailType == mailRequest.MailType)?.TemplatePath;
      if (string.IsNullOrEmpty(templatePath))
        throw new Exception("No template path defined for this email type");
      return $"{Directory.GetCurrentDirectory()}\\Templates\\Emails\\{templatePath}";
    }
    return string.Empty;
  }
  public static MailTypeEnum GetMailTypeEnum(string mailType)
  {
    return (MailTypeEnum)Enum.Parse(typeof(MailTypeEnum), mailType, true);
  }
}
