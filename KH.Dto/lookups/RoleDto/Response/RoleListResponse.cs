using KH.Domain.Entities;

namespace KH.Dto.lookups.RoleDto.Response;

public class RoleListResponse : BasicEntityDto
{
  //we used light BasicEntityDto because in pagination list we may
  //not need the tracking info like created by ..etc
  //we can use auto mapper to do mapping or doing our own using ctor

  public RoleListResponse()
  {
  }

  public RoleListResponse(Role e)
  {
    NameAr = e.NameAr;
    NameEn = e.NameEn;
    Description = e.Description;
  }

}
