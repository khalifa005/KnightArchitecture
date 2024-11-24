using Microsoft.AspNetCore.Http;

namespace KH.Dto.Models.MediaDto.Request;

public class PdfMergeRequest
{
  public IFormFile Pdf1 { get; set; }
  public IFormFile Pdf2 { get; set; }
  public IFormFile Pdf3 { get; set; }
}
