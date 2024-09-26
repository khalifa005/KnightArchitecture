using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;
using KH.Dto.Models.MediaDto.Request;
using KH.Dto.Models.MediaDto.Response;
using KH.Helper.Extentions;
using KH.Helper.Responses;
using KH.Services.Contracts;
using KH.Services.Features;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EmailTrackerResponse>>> Get(int id)
    {
      var res = await _emailService.GetAsync(id);
      return AsActionResult(res);
    }

    [HttpPost("list")]
    public async Task<ActionResult<ApiResponse<PagedResponse<EmailTrackerResponse>>>> GetList([FromBody] MailRequest request)
    {
      var res = await _emailService.GetListAsync(request);
      return AsActionResult(res);
    }


    [HttpPost("Send")]
    public async Task<IActionResult> SendOrderEmail([FromForm] MailRequest request)
    {
      await _emailService.SendEmailAsync(request);

      return Ok("Confirmation Email Sent!");
    }
  }
}
