
namespace KH.Dto.lookups.DepartmentDto.Response
{
  public class DepartmentResponse : BasicEntityWithTrackingDto
  {
    //there is no cutom props because it's lookup and common dto has all needed props
    //we can use auto mapper to do mapping or doing our own using ctor

    public DepartmentResponse()
    {
    }

    public DepartmentResponse(Department e)
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
