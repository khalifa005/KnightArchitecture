using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;

namespace KH.WebApi.Controllers;
public class EmailController : BaseApiController
{
  private readonly IEmailService _emailService;

  public EmailController( IEmailService emailService)
  {
    _emailService = emailService;
  }
  [PermissionAuthorize(PermissionKeysConstant.Emails.VIEW_EMAIL)]

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<EmailTrackerResponse>>> Get(int id)
  {
    var res = await _emailService.GetAsync(id);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Emails.LIST_EMAILS)]

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<EmailTrackerResponse>>>> GetList([FromBody] MailRequest request)
  {
    var res = await _emailService.GetListAsync(request);
    return AsActionResult(res);
  }

  [PermissionAuthorize(PermissionKeysConstant.Emails.SEND_EMAIL)]
  [HttpPost("Send")]
  public async Task<IActionResult> SendEmail([FromForm] MailRequest request)
  {
    await _emailService.SendEmailAsync(request);

    return Ok("Confirmation Email Sent!");
  }
}

