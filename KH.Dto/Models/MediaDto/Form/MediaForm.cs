using Microsoft.AspNetCore.Http;

namespace KH.Dto.Models.MediaDto.Form
{
  public class MediaForm
  {
    public long? Id { get; set; }
    public string? FileName { get; set; }
    public string? OrignalName { get; set; }
    public string? ContentType { get; set; }
    public string? Path { get; set; }
    public string? Extention { get; set; }
    //related to ModelEnum.ToString()
    public string? Model { get; set; }
    //as fake FK of the related item
    public int? ModelId { get; set; }
    public IFormFile? File { get; set; }
    public IFormFileCollection? Files { get; set; }
  }

}
