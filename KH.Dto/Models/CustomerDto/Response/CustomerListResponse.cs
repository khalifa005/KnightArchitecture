namespace KH.Dto.Models.CustomerDto.Response
{
  public class CustomerListResponse
  {
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? FullName { get { return FirstName + " " + LastName; } }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public int IDType { get; set; }
    public string? IDNumber { get; set; }
    public string? Email { get; set; }
    public bool IsSelfRegistered { get; set; }
  }

}
