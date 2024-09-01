using System.Text.RegularExpressions;
using KH.Domain.Entities;
using KH.Domain.Entities.lookups;

namespace KH.Dto.lookups.RoleDto.Response
{
  public class RoleResponse : BasicEntityWithTrackingDto
  {
    //there is no cutom props because it's lookup and common dto has all needed props
    //we can use auto mapper to do mapping or doing our own using ctor

    public RoleResponse()
    {
    }

    public RoleResponse(Role e)
    {
      NameAr = e.NameAr;
      NameEn = e.NameEn;
      Description = e.Description;
      CreatedDate = e.CreatedDate;
      UpdatedDate = e.UpdatedDate;
      CreatedById = e.CreatedById;
    }


  }

}
