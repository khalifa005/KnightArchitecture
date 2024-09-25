using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;
using KH.Helper.Extentions;
using KH.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace KH.WebApi.Controllers
{
  public class EmailController : BaseApiController
  {
    public readonly IUserService _userService;
    private readonly IEmailService _emailService;

    public EmailController(IUserService userService, IEmailService emailService)
    {
      _userService = userService;
      _emailService = emailService;
    }

    [HttpPost("Send")]
    public async Task<IActionResult> SendOrderEmail([FromForm] MailRequest request)
    {
      var orderModel = new OrderConfirmationModel
      {
        UserName = "John Doe",
        OrderId = "ORDER12345",
        InvoiceLink = "https://yourcompany.com/invoice/ORDER12345",
        Products = new List<ProductModel>
            {
                new ProductModel { Name = "Product A", Price = 50.00, Quantity = 1 },
                new ProductModel { Name = "Product B", Price = 25.00, Quantity = 2 }
            }
      };

      //await _emailService.SendOrderConfirmationAsync(orderModel);
      await _emailService.SendEmailAsync(request);

      return Ok("Order confirmation email sent!");
    }
  }
}
