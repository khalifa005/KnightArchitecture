using KH.Domain.Entities;

namespace KH.Dto.Models.EmailDto.Response
{
  public class EmailTrackerResponse : BaseEntityDto
  {
    public string Model { get; set; }
    public long ModelId { get; set; }
    public string ToEmail { get; set; }
    public string ToCCEmail { get; set; }
    public string Subject { get; set; }
    public string? Body { get; set; }
    public string MailType { get; set; }
    public bool IsSent { get; set; }
    public string? FailReasons { get; set; }

    public EmailTrackerResponse()
    {
          
    }

    public EmailTrackerResponse(EmailTracker e)
    {
      Id = e.Id;
      Model = e.Model;
      ModelId = e.ModelId;
      ToEmail = e.ToEmail;
      ToCCEmail = e.ToCCEmail;
      Subject = e.Subject;
      Body = e.Body;
      MailType = e.MailType;
      IsSent = e.IsSent;
      FailReasons = e.FailReasons;
      
    }
  }
}
