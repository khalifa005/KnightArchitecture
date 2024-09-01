namespace KH.Dto.Models.MediaDto.Request
{
  public class MediaRequest : BasicTrackerEntityDto
  {
    public int? Id { get; set; }
    public string? FileName { get; set; }
    public string? OrignalName { get; set; }
    public string? Path { get; set; }
    public string? Model { get; set; }
    public int? ModelId { get; set; }
  }

}
