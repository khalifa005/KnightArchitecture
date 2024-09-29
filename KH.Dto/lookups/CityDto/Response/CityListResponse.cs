namespace KH.Dto.lookups.CityDto.Response;

public class DepartmentListResponse : BasicEntityDto
{
  //we used light BasicEntityDto because in pagination list we may
  //not need the tracking info like created by ..etc
  //we can use auto mapper to do mapping or doing our own using ctor

  public DepartmentListResponse()
  {
  }

  public DepartmentListResponse(KH.Domain.Entities.lookups.City e)
  {
    NameAr = e.NameAr;
    NameEn = e.NameEn;
    Description = e.Description;
  }

}
