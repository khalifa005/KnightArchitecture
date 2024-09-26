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
      await _emailService.SendEmailAsync(request);

      return Ok("Confirmation Email Sent!");
    }
  }
}
