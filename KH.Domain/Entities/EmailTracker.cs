
namespace KH.Domain.Entities
{
  public class EmailTracker : BaseEntity
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

  }


}
