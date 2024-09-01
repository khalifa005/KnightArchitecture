using Microsoft.AspNetCore.Mvc;

namespace KH.Dto.Models.MediaDto.Response
{
  public class MediaResponse
  {
    public FileContentResult? FileContentResult { get; set; }
    public bool IsValidToDownload { get; set; }
    public bool IsDeleted { get; set; }
    public string Message { get; set; }
    public string FileName { get; set; }
    public string OrignalName { get; set; }
    public string FileExtention { get; set; }
    public string FilePath { get; set; }

  }
}
