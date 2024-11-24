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

  [HttpPost("GenerateWithQuest")]
  public async Task<IActionResult> GeneratePdf([FromBody] UserFilterRequest request, CancellationToken cancellationToken)
  {
    //var pdfBytes = await _pdfService.GeneratePdfAsync(request, cancellationToken);
    var pdfBytes = await _pdfService.GenerateInvoicePdfWithQuestAsync();
    return FileOrBadRequest(pdfBytes, "Welcome");
  }

  [HttpPost("MergeMultiplePdfs")]
  public async Task<IActionResult> MergePdfs([FromForm] PdfMergeRequest request)
  {
    var pdfBytesList = new List<byte[]>
        {
            await ReadFileBytesAsync(request.Pdf1),
            await ReadFileBytesAsync(request.Pdf2),
            await ReadFileBytesAsync(request.Pdf3)
        };

    var mergedPdfBytes = await _pdfService.MergePdfsAsync(pdfBytesList);
    return FileOrBadRequest(mergedPdfBytes, "MergedDocument");

  }

  private async Task<byte[]> ReadFileBytesAsync(IFormFile file)
  {
    using var memoryStream = new MemoryStream();
    await file.CopyToAsync(memoryStream);
    return memoryStream.ToArray();
  }


}
