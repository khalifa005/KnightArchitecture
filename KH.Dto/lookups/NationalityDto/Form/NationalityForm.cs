
namespace KH.Dto.lookups.NationalityDto.Form
{
  public class NationalityForm : BasicEntityWithTrackingDto
  {
    //there is no cutom props because it's lookup and common dto has all needed props
    //we can use auto mapper to do mapping or doing our own using ctor
    public bool IsUpdateMode { get; set; }
    public NationalityForm()
    {
    }
    public NationalityForm(Nationality e)
    {
      NameAr = e.NameAr;
      NameEn = e.NameEn;
      Description = e.Description;
      CreatedDate = e.CreatedDate;
      UpdatedDate = e.UpdatedDate;
      CreatedById = e.CreatedById;
    }
    public Nationality ToEntity()
    {
      var e = new Nationality()
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