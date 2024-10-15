using KH.Domain.Entities;

namespace KH.Dto.Lookups.PermissionsDto.Request;

public class CreatePermissionRequest : LookupEntityWithTrackingDto
{
  public bool IsUpdateMode { get; set; }
  public int SortKey { get; set; }
  public long? ParentId { get; set; }

  public CreatePermissionRequest()
  {
  }
  public CreatePermissionRequest(Permission e)
  {
    NameAr = e.NameAr;
    NameEn = e.NameEn;
    SortKey = e.SortKey;
    ParentId = e.ParentId;
    Description = e.Description;
    CreatedDate = e.CreatedDate;
    UpdatedDate = e.UpdatedDate;
    CreatedById = e.CreatedById;
  }
  public Permission ToEntity()
  {
    var e = new Permission()
    {
      NameAr = NameAr,
      NameEn = NameEn,
      SortKey = SortKey,
      ParentId = ParentId,
      Description = Description,
    };

    if (Id.HasValue)
    {
      //update mode
      IsUpdateMode = true;
      e.Id = Id.Value;
    }
    else
    {
      //e.Id = Id.Value;
    }

    return e;
  }

}
