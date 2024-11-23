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
    var pdfBytes = await _pdfService.ExportUserDetailsPdfAsync(request, cancellationToken);

    if (pdfBytes is object && pdfBytes.Length > 0)
    {
      return File(pdfBytes, "application/pdf", $"UserDetails_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.pdf");

    }

    return BadRequest("invalid-parameters");
  }

  [PermissionAuthorize(PermissionKeysConstant.Pdf.EXPORT_PDF)]

  [HttpPost("DownloadBasicTemplate")]

  public async Task<IActionResult> DownloadBasicTemplate([FromBody] UserFilterRequest request, CancellationToken cancellationToken)
  {
    var pdfBytes = await _pdfService.GeneratePdf("");

    if (pdfBytes is object && pdfBytes.Length > 0)
    {
      return File(pdfBytes, "application/pdf", $"UserDetails_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.pdf");

    }

    return BadRequest("invalid-parameters");
  }


  [PermissionAuthorize(PermissionKeysConstant.Pdf.EXPORT_PDF)]

  [HttpPost("DownloadInvoiceTemplate")]

  public async Task<IActionResult> DownloadInvoiceTemplate([FromBody] UserFilterRequest request, CancellationToken cancellationToken)
  {
    var pdfBytes = await _pdfService.GenerateInvoicePdf();

    if (pdfBytes is object && pdfBytes.Length > 0)
    {
      return File(pdfBytes, "application/pdf", $"UserDetails_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.pdf");

    }

    return BadRequest("invalid-parameters");
  }

}
