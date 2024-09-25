using KH.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace KH.Dto.Models.EmailDto.Request
{
  public class MailRequest
  {
    public List<string?>? ToEmail { get; set; }
    public List<string?>? ToCCEmail { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public List<IFormFile>? Attachments { get; set; }
    public MailTypeEnum MailType { get; set; } = MailTypeEnum.Default;
    public int MailTypeId { get; set; }
    public long UserId { get; set; }

  }
}
