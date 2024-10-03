
namespace KH.WebApi.Controllers;

public class AuditsController : BaseApiController
{
  public readonly IAuditService _auditService;
  public AuditsController(IAuditService  auditService)
  {
    _auditService = auditService;
  }

  [HttpGet("GetUserAudits/{userId}")]
  public async Task<ActionResult<ApiResponse<List<AuditResponse>>>> Get(string userId)
  {
    var res = await _auditService.GetCurrentUserTrailsAsync(userId);
    return AsActionResult(res);
  }
}

