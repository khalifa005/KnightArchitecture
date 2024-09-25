namespace KH.Dto.Models.EmailDto.Response
{
  public class MailRequestFollowUp 
  {
    public bool IsSent { get; set; }
    public string ToEmail { get; set; }
    public int ModelId { get; set; }
    public string Model { get; set; }
    public string? ToCCEmail { get; set; }
    public string? FailReasons { get; set; }
    public string Subject { get; set; }
    public string? Body { get; set; }
    public string MailType { get; set; }
    
  }

}
