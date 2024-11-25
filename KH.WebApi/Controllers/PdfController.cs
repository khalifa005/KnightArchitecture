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

  [HttpPost("WelcomePDFTemplateFromHTML")]
  [PermissionAuthorize(PermissionKeysConstant.Pdf.EXPORT_PDF)]
  public async Task<IActionResult> WelcomePDFTemplateFromHtml([FromBody] UserFilterRequest request, CancellationToken cancellationToken)
  {
    var pdfBytes = await _pdfService.ExportUserDetailsPdfAsync(request, cancellationToken);
    return FileOrBadRequest(pdfBytes, "UserDetails");
  }

  [HttpPost("InvoicePDFTemplateFromHTML")]
  [PermissionAuthorize(PermissionKeysConstant.Pdf.EXPORT_PDF)]
  public async Task<IActionResult> InvoicePDFTemplateFromHTML([FromBody] UserFilterRequest request, CancellationToken cancellationToken)
  {
    var pdfBytes = await _pdfService.ExportUserInvoicePdf(request.Language);
    return FileOrBadRequest(pdfBytes, "Invoice");
  }

  [HttpPost("InvoicePDFTemplateWithQuest")]
  public async Task<IActionResult> InvoicePDFTemplateWithQuest([FromBody] UserFilterRequest request, CancellationToken cancellationToken)
  {
    //var pdfBytes = await _pdfService.GeneratePdfAsync(request, cancellationToken);
    var pdfBytes = await _pdfService.GenerateInvoicePdfWithQuestAsync();
    return FileOrBadRequest(pdfBytes, "Welcome");
  }

  [HttpPost("GenerateWithNreco")]
  public async Task<IActionResult> GenerateWithNreco()
  {
    var pdfBytes =  _pdfService.GeneratePdfFromHtmlWithNReco();
    return FileOrBadRequest(pdfBytes, "Welcome");
  }


  [HttpPost("MergeMultiplePdfsWithSharp")]
  public async Task<IActionResult> MergeMultiplePdfsWithSharp([FromForm] List<IFormFile> files)
  {
    if (files == null || files.Count == 0)
      return BadRequest("No files were provided.");

    var mergedPdfBytes = await _pdfService.MergePdfsAsync(files);
    return FileOrBadRequest(mergedPdfBytes, "MergedDocument");
  }

  private IActionResult FileOrBadRequest(byte[] pdfBytes, string filePrefix)
  {
    if (pdfBytes.Length > 0)
    {
      var fileName = $"{filePrefix}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";
      return File(pdfBytes, "application/pdf", fileName);
    }

    return BadRequest("Invalid file.");
  }
}
