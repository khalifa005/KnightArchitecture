namespace KH.Dto.lookups.NationalityDto.Response
{
  public class NationalityResponse : LookupEntityWithTrackingDto
  {
    //there is no cutom props because it's lookup and common dto has all needed props
    //we can use auto mapper to do mapping or doing our own using ctor

    public NationalityResponse()
    {
    }

    public NationalityResponse(Nationality e)
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
