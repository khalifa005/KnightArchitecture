namespace KH.Dto.lookups.Group.Response
{
  public class GroupListResponse : BasicEntityDto
  {
    //we used light BasicEntityDto because in pagination list we may
    //not need the tracking info like created by ..etc
    //we can use auto mapper to do mapping or doing our own using ctor

    public GroupListResponse()
    {
    }

    public GroupListResponse(KH.Domain.Entities.lookups.Group e)
    {
      NameAr = e.NameAr;
      NameEn = e.NameEn;
      Description = e.Description;
    }

  }

}
