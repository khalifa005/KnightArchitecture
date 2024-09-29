using DinkToPdf;
using DinkToPdf.Contracts;
namespace KH.Services.Features
{
  public class PdfService : IPdfService
  {
    private readonly IUserService _userService;
    private readonly IConverter _converter;
    //private readonly IRazorViewToStringRenderer _razorRenderer; // Inject Razor renderer

    public PdfService(IUserService userService, IConverter converter)
    {
      _userService = userService;
      _converter = converter;
      //_razorRenderer = razorRenderer;
    }

    public async Task<byte[]> ExportUserDetailsPdfAsync(UserFilterRequest param)
    {
      var response = await _userService.GetAsync(param.Id.Value);
      var entityFromDB = response.Data;

      string filePathForTemplateBody = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PDF", "CustomerPdfTemplateBody.html");
      string htmlContentForBody = ReplaceUserPlaceholders(entityFromDB, filePathForTemplateBody);

      string filePathForTemplateLayout = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PDF", "CustomerPdfLayout.html");
      string htmlFileContentForLayout = await File.ReadAllTextAsync(filePathForTemplateLayout);

      htmlFileContentForLayout = htmlFileContentForLayout.Replace("{Htmlbody}", htmlContentForBody);


      // Convert the HTML content to PDF using DinkToPdf
      var pdfDocument = new HtmlToPdfDocument()
      {
        GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A3Extra,
                Out = null, // Out set to null because we want the result as a byte array
            },
        Objects = {
                new ObjectSettings() {
                    HtmlContent = htmlFileContentForLayout,
                    WebSettings = { DefaultEncoding = "utf-8" },
                }
            }
      };

      // Generate PDF as byte array
      return _converter.Convert(pdfDocument);
    }

    private string ReplaceUserPlaceholders(UserDetailsResponse user, string filePath)
    {
      string htmlFileContent = File.ReadAllText(filePath);

      // Replace placeholders with user details
      htmlFileContent = htmlFileContent.Replace("{FullName}", user.FullName)
                                       .Replace("{Email}", user.Email)
                                       .Replace("{MobileNumber}", user.MobileNumber)
                                       .Replace("{Username}", user.Username)
                                       .Replace("{BirthDate}", user.BirthDate?.ToString("dd/MM/yyyy") ?? "N/A")
                                       .Replace("{OtherDetails}", $"Roles: {string.Join(", ", user.UserRoles.Select(r => r.Role.NameEn))}");

      return htmlFileContent;
    }
  }



}

