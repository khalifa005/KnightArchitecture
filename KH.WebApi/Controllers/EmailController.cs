using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Services.Emails.Contracts;

namespace KH.WebApi.Controllers;
public class EmailController : BaseApiController
{
  private readonly IEmailService _emailService;
  private readonly IEmailTrackerQueryService _emailTrackerQueryService;

  public EmailController(IEmailService emailService, IEmailTrackerQueryService emailTrackerQueryService)
  {
    _emailService = emailService;
    _emailTrackerQueryService = emailTrackerQueryService;
  }
  [PermissionAuthorize(PermissionKeysConstant.Emails.VIEW_EMAIL)]

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<EmailTrackerResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    var res = await _emailTrackerQueryService.GetAsync(id, cancellationToken  );
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Emails.LIST_EMAILS)]

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedList<EmailTrackerResponse>>>> GetList([FromBody] MailRequest request, CancellationToken cancellationToken)
  {
    var res = await _emailTrackerQueryService.GetListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [PermissionAuthorize(PermissionKeysConstant.Emails.SEND_EMAIL)]
  [HttpPost("Send")]
  public async Task<ActionResult<ApiResponse<string>>> SendEmail([FromForm] MailRequest request, CancellationToken cancellationToken)
  {
    var res = await _emailService.SendEmailAsync(request, cancellationToken);

    return AsActionResult(res);
  }
}

