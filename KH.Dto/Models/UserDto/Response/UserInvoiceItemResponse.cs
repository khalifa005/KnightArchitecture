namespace KH.Dto.Models.UserDto.Response;

public partial class UserRoleResponse
{
  public class UserInvoiceItemResponse
  {
    public int UserId { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
  }

}
