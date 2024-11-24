using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Services.Media_s.Contracts;

namespace KH.WebApi.Controllers;

public class PdfController : BaseApiController
{
  private readonly IPdfService _pdfService;

  public PdfController(IPdfService pdfService)
  {
    _pdfService = pdfService;
  }

  [HttpPost("Download")]
  [PermissionAuthorize(PermissionKeysConstant.Pdf.EXPORT_PDF)]
  public async Task<IActionResult> Download([FromBody] UserFilterRequest request, CancellationToken cancellationToken)
  {
    var pdfBytes = await _pdfService.ExportUserDetailsPdfAsync(request, cancellationToken);
    return FileOrBadRequest(pdfBytes, "UserDetails");
  }

  [HttpPost("DownloadInvoiceTemplate")]
  [PermissionAuthorize(PermissionKeysConstant.Pdf.EXPORT_PDF)]
  public async Task<IActionResult> DownloadInvoiceTemplate([FromBody] UserFilterRequest request, CancellationToken cancellationToken)
  {
    var pdfBytes = await _pdfService.ExportUserInvoicePdf(request.Language);
    return FileOrBadRequest(pdfBytes, "Invoice");
  }

  private IActionResult FileOrBadRequest(byte[] pdfBytes, string filePrefix)
  {
    if (pdfBytes.Length > 0)
    {
      var fileName = $"{filePrefix}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";
      return File(pdfBytes, "application/pdf", fileName);
    }

    return BadRequest("Invalid parameters.");
  }
}
