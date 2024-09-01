namespace KH.Dto.lookups.GroupDto.Form
{
  public class GroupForm : BasicEntityWithTrackingDto
  {
    //there is no cutom props because it's lookup and common dto has all needed props
    //we can use auto mapper to do mapping or doing our own using ctor
    public long? TicketCategoryId { get; set; }
    public bool IsUpdateMode { get; set; }

    public GroupForm()
    {
    }

    public GroupForm(KH.Domain.Entities.lookups.Group e)
    {
      TicketCategoryId = e.TicketCategoryId;
      NameAr = e.NameAr;
      NameEn = e.NameEn;
      Description = e.Description;
      CreatedDate = e.CreatedDate;
      UpdatedDate = e.UpdatedDate;
      CreatedById = e.CreatedById;
    }

    public City ToEntity()
    {
      var e = new City()
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
        //creation mode
      }

      return e;
    }



  }

}
