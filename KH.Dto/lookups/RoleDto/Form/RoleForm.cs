
using KH.Domain.Entities;

namespace KH.Dto.lookups.RoleDto.Form;

public class RoleForm : LookupEntityWithTrackingDto
{
  public bool IsUpdateMode { get; set; }
  public RoleForm()
  {
  }
  public RoleForm(Role e)
  {
    NameAr = e.NameAr;
    NameEn = e.NameEn;
    Description = e.Description;
    CreatedDate = e.CreatedDate;
    UpdatedDate = e.UpdatedDate;
    CreatedById = e.CreatedById;
  }
  public Role ToEntity()
  {
    var e = new Role()
    {
      NameAr = NameAr,
      NameEn = NameEn,
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
      e.Id = Id.Value;
    }

    return e;
  }

}
