using KH.Domain.Entities;

namespace KH.Dto.Models.UserDto.Form
{
  public class UserForm
  {

    #region Props

    public long? Id { get; set; }
    public bool? IsUpdateMode { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? MobileNumber { get; set; }
    public int? GroupId { get; set; }
    public int? DepartmentId { get; set; }
    public int[]? RoleIds { get; set; }

    #endregion

    #region MappingMethods
    public UserForm(User e)
    {
    }
    public User ToEntity()
    {
      var e = new User()
      {
        //mapp all needed props - or use automapper
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

    #endregion
  }

}
