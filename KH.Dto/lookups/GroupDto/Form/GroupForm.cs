namespace KH.Dto.lookups.GroupDto.Form;

public class GroupForm : LookupEntityWithTrackingDto
{
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

  public Group ToEntity()
  {
    var e = new Group()
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
