
namespace KH.Dto.lookups.PolicyIssuingSourceDto.Form
{
  public class PolicyIssuingSourceForm : LookupEntityWithTrackingDto
  {
    //there is no cutom props because it's lookup and common dto has all needed props
    //we can use auto mapper to do mapping or doing our own using ctor
    public bool IsUpdateMode { get; set; }
    public PolicyIssuingSourceForm()
    {
    }
    public PolicyIssuingSourceForm(PolicyIssuingSource e)
    {
      NameAr = e.NameAr;
      NameEn = e.NameEn;
      Description = e.Description;
      CreatedDate = e.CreatedDate;
      UpdatedDate = e.UpdatedDate;
      CreatedById = e.CreatedById;
    }
    public PolicyIssuingSource ToEntity()
    {
      var e = new PolicyIssuingSource()
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
