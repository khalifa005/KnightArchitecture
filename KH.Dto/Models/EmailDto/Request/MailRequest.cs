using KH.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace KH.Dto.Models.EmailDto.Request
{
  public class MailRequest
  {
    /// <summary>
    /// Emails Of Recipients
    /// </summary>
    public List<string?>? ToEmail { get; set; }

    /// <summary>
    /// Emails Of CC
    /// </summary>
    public List<string?>? ToCCEmail { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public List<IFormFile>? Attachments { get; set; }
    public MailTypeEnum MailType { get; set; } = MailTypeEnum.Default;

    /// <summary>
    /// MUST be Have Value When MailType Be IN [TicketEscalation Or TicketEscalationComments]
    /// </summary>
    public int? FilterTicketId { get; set; }

    /// <summary>
    /// SET True When SEND Email On Ticket Creation
    /// </summary>
    public bool IsCreationLevelTicket { get; set; }

    /// <summary>
    /// SET True When SEND Email On Ticket Change Department
    /// </summary>
    public bool IsChangedDepartment { get; set; }
  }
}
