namespace KH.WebApi.Controllers;

public class PdfController : BaseApiController
{
  public readonly IUserService _userService;
  private readonly IPdfService _pdfService;

  public PdfController(IUserService userService, IPdfService pdfService)
  {
    _userService = userService;
    _pdfService = pdfService;
  }

  [HttpPost("Download")]

  public async Task<IActionResult> Download([FromBody] UserFilterRequest request)
  {
    try
    {
      var pdfBytes = await _pdfService.ExportUserDetailsPdfAsync(request);
      return File(pdfBytes, "application/pdf", $"UserDetails_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.pdf");
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}
