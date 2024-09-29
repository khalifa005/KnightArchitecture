using KH.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KH.Dto.Models.MediaDto.Response;

public class MediaResponse : BasicTrackerEntityDto
{
  public FileContentResult? FileContentResult { get; set; }
  public bool IsValidToDownload { get; set; }
  public bool IsDeleted { get; set; }
  public string Message { get; set; }
  public string FileName { get; set; }
  public string OrignalName { get; set; }
  public string FileExtention { get; set; }
  public string? ContentType { get; set; }
  public string? Model { get; set; }
  //as fake FK of the related item
  public int? ModelId { get; set; }
  public string FilePath { get; set; }

  public MediaResponse()
  {

  }

  public MediaResponse(Media entity)
  {
    Id = entity.Id;
    FileName = entity.FileName;
    OrignalName = entity.OrignalName;
    FileExtention = entity.Extention;
    FilePath = entity.Path;
    IsDeleted = entity.IsDeleted;
    CreatedById = entity.CreatedById;
    CreatedDate = entity.CreatedDate;
    UpdatedById = entity.UpdatedById;
    UpdatedDate = entity.UpdatedDate;

  }

}
