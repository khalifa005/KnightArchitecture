
namespace KH.Dto.lookups.DepartmentDto.Response;

public class DepartmentResponse : LookupEntityWithTrackingDto
{
  //there is no cutom props because it's lookup and common dto has all needed props
  //we can use auto mapper to do mapping or doing our own using ctor

  public DepartmentResponse()
  {
  }

  public DepartmentResponse(Department e)
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
