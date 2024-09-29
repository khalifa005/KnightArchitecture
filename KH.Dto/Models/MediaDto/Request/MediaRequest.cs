namespace KH.Dto.Models.MediaDto.Request;

public class MediaRequest : PagingRequestHelper
{
  public string? FileName { get; set; }
  public string? OrignalName { get; set; }
  public string? Path { get; set; }
  public string? Model { get; set; }
  public long? ModelId { get; set; }
}
