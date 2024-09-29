using Microsoft.AspNetCore.Mvc;

namespace KH.BuildingBlocks.Responses
{
  public class FileResponse
  {

    /// <summary>
    /// for internal uses in file manager
    /// </summary>
    public FileContentResult? FileContentResult { get; set; }
    public bool IsValid { get; set; }
    public bool IsDeleted { get; set; }
    public string? Message { get; set; }
    public string? ContentType { get; set; }
    public string? GeneratedFileName { get; set; }
    public string? OrignalFileName { get; set; }
    public string? FileExtention { get; set; }
    public string? FilePath { get; set; }
    public string? FileId { get; set; } //custom

  }
}
