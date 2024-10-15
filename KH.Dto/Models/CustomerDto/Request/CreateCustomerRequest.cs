using KH.Domain.Entities;

namespace KH.Dto.Models.CustomerDto.Request;

public class CreateCustomerRequest
{
  #region Props
  public long? Id { get; set; }

  public string? FirstName { get; set; }
  public string? MiddleName { get; set; }
  public string? LastName { get; set; }
  public string? MobileNumber { get; set; }
  public int IDType { get; set; }
  public string? IDNumber { get; set; }
  public string? PassportNumber { get; set; }
  public string? Email { get; set; }
  public string? Username { get; set; }
  public string? Password { get; set; }
  public DateTime? BirthDate { get; set; }
  public bool IsSelfRegistered { get; set; }
  public bool IsUpdateMode { get; set; }


  #endregion

  #region MappingMethods
  public CreateCustomerRequest(Customer e)
  {
  }
  public Customer ToEntity()
  {
    var e = new Customer()
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
