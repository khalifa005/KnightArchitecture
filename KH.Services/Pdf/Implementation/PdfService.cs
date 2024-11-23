namespace KH.Services.Pdf.Implementation;

public class PdfService : IPdfService
{
  private readonly IUserQueryService _userService;
  private readonly IConverter _converter;
  private readonly ILogger<PdfService> _logger;
  //private readonly IRazorViewToStringRenderer _razorRenderer; // Inject Razor renderer

  public PdfService(IUserQueryService userService, IConverter converter, ILogger<PdfService> logger)
  {
    _userService = userService;
    _converter = converter;
    _logger = logger;
    //_razorRenderer = razorRenderer;
  }

  public async Task<byte[]> ExportUserDetailsPdfAsync(UserFilterRequest param, CancellationToken cancellationToken)
  {
    try
    {
      //we may want to pass the user data with the template name as params but this is a demo to be standalone
      var response = await _userService.GetAsync(param.Id.Value, cancellationToken);

      if (response.StatusCode != StatusCodes.Status200OK)
      {
        return Array.Empty<byte>();
      }

      var entityFromDB = response.Data;

      string filePathForTemplateBody = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PDF", "CustomerPdfTemplateBody.html");
      string htmlContentForBody = ReplaceUserPlaceholders(entityFromDB, filePathForTemplateBody);

      if (string.IsNullOrEmpty(htmlContentForBody))
        throw new ArgumentException("HTML content cannot be null or empty.", nameof(htmlContentForBody));


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
              //Setting Out = null means that the library will return the PDF as a byte array instead of saving it to a file on disk.
              Out = null,

          },
        Objects = {
              new ObjectSettings() {
                  HtmlContent = htmlFileContentForLayout,
                  WebSettings = {
                  DefaultEncoding = "utf-8"
                  //UserStyleSheet = null // Optionally add custom CSS
                },
              }
          }
      };

      // Generate PDF as byte array
      return _converter.Convert(pdfDocument);
    }
    catch (Exception ex)
    {
      _logger.LogError($"pdf Exception in {ex.Message}: {ex.InnerException.Message}");
      return Array.Empty<byte>();
    }


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

  public async Task<byte[]> GeneratePdf(string htmlContent)
  {
    //this will handle all PDF export just pass the HTML content and it will take care 

    string filePathForTemplateLayout = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PDF", "BasicHtmlTemplate.html");
    string htmlFileContentForLayout = await File.ReadAllTextAsync(filePathForTemplateLayout);


    if (string.IsNullOrEmpty(htmlContent))
      throw new ArgumentException("HTML content cannot be null or empty.", nameof(htmlContent));

    // Configure the PDF document
    var pdfDocument = new HtmlToPdfDocument
    {
      GlobalSettings =
      {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4, // Standard A4 paper size
        Out = null // Return as a byte array
      },
      Objects =
            {
                new ObjectSettings
                {
                    HtmlContent = htmlContent,
                    WebSettings = new WebSettings
                    {
                        DefaultEncoding = "utf-8",
                        UserStyleSheet = null // Optionally add custom CSS
                    },
                    HeaderSettings = new HeaderSettings
                    {
                        FontSize = 10,
                        Right = "Page [page] of [toPage]",
                        Line = true
                    },
                    FooterSettings = new FooterSettings
                    {
                        FontSize = 10,
                        Center = "Generated on [date]"
                    }
                }
            }
    };

    // Convert the HTML to PDF and return the result
    return _converter.Convert(pdfDocument);
  }

}
