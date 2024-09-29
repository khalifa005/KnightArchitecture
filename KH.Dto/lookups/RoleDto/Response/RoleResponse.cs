using KH.Domain.Entities;

namespace KH.Dto.lookups.RoleDto.Response
{
  public class RoleResponse : LookupEntityWithTrackingDto
  {
    //there is no cutom props because it's lookup and common dto has all needed props
    //we can use auto mapper to do mapping or doing our own using ctor

    public RoleResponse()
    {
    }

    public RoleResponse(Role e)
    {
      Id = e.Id;
      NameAr = e.NameAr;
      NameEn = e.NameEn;
      Description = e.Description;
      CreatedDate = e.CreatedDate;
      UpdatedDate = e.UpdatedDate;
      CreatedById = e.CreatedById;
    }


  }

}
