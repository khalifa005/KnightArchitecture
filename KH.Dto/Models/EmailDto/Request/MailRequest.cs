using KH.BuildingBlocks.Enums;
using KH.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace KH.Dto.Models.EmailDto.Request
{
  public class MailRequest : PagingRequestHelper
  {
    public List<string?>? ToEmail { get; set; }
    public List<string?>? ToCCEmail { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public List<IFormFile>? Attachments { get; set; }
    public MailTypeEnum MailType { get; set; } = MailTypeEnum.Default;
    public long ModelId { get; set; }
    public string Model { get; set; }
    public bool IsSent { get; set; }

    public MailRequest()
    {

    }
    public EmailTracker ToEntity()
    {
      var e = new EmailTracker();

      e.ToEmail = string.Join(",", ToEmail);
      e.ToCCEmail = string.Join(",", ToCCEmail);
      e.Subject = Subject;
      e.Body = Body;
      e.MailType = MailType.ToString();
      e.Model = Model;
      e.ModelId = ModelId;

      return e;
    }

  }
}
