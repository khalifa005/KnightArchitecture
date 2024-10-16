using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Services.Media_s.Contracts;

namespace KH.WebApi.Controllers;

public class PdfController : BaseApiController
{
  public readonly IUserQueryService _userService;
  private readonly IPdfService _pdfService;

  public PdfController(IUserQueryService userService, IPdfService pdfService)
  {
    _userService = userService;
    _pdfService = pdfService;
  }
  [PermissionAuthorize(PermissionKeysConstant.Pdf.EXPORT_PDF)]

  [HttpPost("Download")]

  public async Task<IActionResult> Download([FromBody] UserFilterRequest request, CancellationToken cancellationToken)
  {
    try
    {
      var pdfBytes = await _pdfService.ExportUserDetailsPdfAsync(request, cancellationToken);
      return File(pdfBytes, "application/pdf", $"UserDetails_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.pdf");
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}
