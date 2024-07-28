using KH.Domain.Entities.lookups;
using KH.Dto.common;

namespace KH.Dto.lookups
{
  public class CityResponseDto : BasicEntityDto
  {

    public CityResponseDto()
    {
    }

    //without auto mapper 
    public CityResponseDto(City e)
    {
      Id = e.Id;
      Description = e.Description;
      NameEn = e.NameEn;
      NameAr = e.NameAr;
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

    #region Props

    public bool IsUpdateMode { get; set; }

    #endregion

    #region FK

    #endregion
  }


}
