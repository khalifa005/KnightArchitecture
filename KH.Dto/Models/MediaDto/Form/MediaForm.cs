using Microsoft.AspNetCore.Http;

namespace KH.Dto.Models.MediaDto.Form
{
  public class MediaForm
  {
    //related to ModelEnum.ToString()
    public string? Model { get; set; }
    //as fake FK of the related item
    public int? ModelId { get; set; }
    public IFormFile? File { get; set; }
    public IFormFileCollection? Files { get; set; }


  }

}
