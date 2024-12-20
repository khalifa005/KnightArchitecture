using Microsoft.AspNetCore.Http;

namespace KH.Dto.Models.MediaDto.Request;

public class CreateMediaRequest
{
  //related to ModelEnum.ToString()
  public string? Model { get; set; }
  //as fake FK of the related item
  public int? ModelId { get; set; }
  public IFormFile? File { get; set; }
  public IFormFileCollection? Files { get; set; }
  public IFormCollection? FormData { get; set; }


}
