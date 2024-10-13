
using KH.BuildingBlocks.Auth.V1;
using KH.BuildingBlocks.Constant;
using System.Net;

namespace KH.WebApi.Controllers;

public class AuditsController : BaseApiController
{
  public readonly IAuditService _auditService;
  public AuditsController(IAuditService auditService)
  {
    _auditService = auditService;
  }

  [PermissionAuthorize(PermissionKeysConstant.Audits.VIEW_AUDITS)]
  [HttpGet("GetUserAudits/{userId}")]
  public async Task<ActionResult<ApiResponse<List<AuditResponse>>>> Get(string userId)
  {
    var res = await _auditService.GetCurrentUserTrailsAsync(userId);
    return AsActionResult(res);
  }

  [PermissionAuthorize(
    PermissionOperator.Or,
    PermissionKeysConstant.SUPER_ADMIN_PERMISSION,
    PermissionKeysConstant.Audits.EXPORT_AUDITS)]

  [HttpGet("ExportUserAudits/{userId}")]
  public async Task<IActionResult> ExportExcel(string userId, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false)
  {
    var res = await _auditService.ExportToExcelAsync(userId, searchString, searchInOldValues, searchInNewValues);
    // Assuming res.Data contains the Base64 encoded string of the Excel file
    var fileContent = Convert.FromBase64String(res.Data);
    var fileName = "AuditTrails.xlsx";

    // Return the file for download
    return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }

  [PermissionAuthorize(PermissionKeysConstant.Audits.IMPORT_AUDITS)]
  [HttpPost("ImportExternalAudit")]
  public async Task<ActionResult<ApiResponse<string>>> ImportExternalAudit([FromForm] MediaForm request)
  {
    if (request.File == null || request.File.Length == 0)
    {
      return new ApiResponse<string>((int)HttpStatusCode.BadRequest, "No file uploaded");
    }

    var res = await _auditService.ImportExternalAudit(request.File);
    return AsActionResult(res);
  }

}

