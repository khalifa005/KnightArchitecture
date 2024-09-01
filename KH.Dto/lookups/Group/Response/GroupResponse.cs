using System.Text.RegularExpressions;
using KH.Domain.Entities.lookups;

namespace KH.Dto.lookups.Group.Response
{
  public class GroupResponse : BasicEntityWithTrackingDto
  {
    //there is no cutom props because it's lookup and common dto has all needed props
    //we can use auto mapper to do mapping or doing our own using ctor

    public GroupResponse()
    {
    }

    public GroupResponse(KH.Domain.Entities.lookups.Group e)
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
