using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Services.Emails.Contracts;

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
  public async Task<ActionResult<ApiResponse<EmailTrackerResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    var res = await _emailService.GetAsync(id, cancellationToken  );
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Emails.LIST_EMAILS)]

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<EmailTrackerResponse>>>> GetList([FromBody] MailRequest request, CancellationToken cancellationToken)
  {
    var res = await _emailService.GetListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [PermissionAuthorize(PermissionKeysConstant.Emails.SEND_EMAIL)]
  [HttpPost("Send")]
  public async Task<IActionResult> SendEmail([FromForm] MailRequest request, CancellationToken cancellationToken)
  {
    await _emailService.SendEmailAsync(request, cancellationToken);

    return Ok("Confirmation Email Sent!");
  }
}

