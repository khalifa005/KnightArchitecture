using KH.BuildingBlocks.Apis.Services;
using System.Text;
using static KH.Dto.Models.UserDto.Response.UserRoleResponse;

namespace KH.Services.Pdf.Implementation;

public class PdfService : IPdfService
{
  private readonly IUserQueryService _userService;
  private readonly IConverter _converter;
  private readonly ILogger<PdfService> _logger;

  public PdfService(IUserQueryService userService,
    IConverter converter,
    ILogger<PdfService> logger)
  {
    _userService = userService;
    _converter = converter;
    _logger = logger;
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


    if (string.IsNullOrEmpty(htmlFileContentForLayout))
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
                    HtmlContent = htmlFileContentForLayout,
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

  
  public async Task<byte[]> GeneratePdfWithDynamicContent(string templatePath, Dictionary<string, string> dynamicContent, string language = "en")
  {
    // Path to layout template
    string layoutFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PDF", "PdfLayoutTemplate.html");
    string layoutContent = await File.ReadAllTextAsync(layoutFilePath);

    string templateContent = await File.ReadAllTextAsync(templatePath);

    if (string.IsNullOrEmpty(layoutContent) || string.IsNullOrEmpty(templateContent))
      throw new ArgumentException("Template content cannot be null or empty.");

    // Replace placeholders in the dynamic content template
    foreach (var placeholder in dynamicContent)
    {
      templateContent = templateContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
    }

    // Determine language direction
    string languageDirection = language == "ar" ? "arabic" : "ltr";

    // Insert populated content and language direction into layout
    string finalHtml = layoutContent
        .Replace("{{CONTENT_PLACEHOLDER}}", templateContent)
        .Replace("{{LANGUAGE_DIRECTION}}", languageDirection)
        .Replace("{{TITLE}}", dynamicContent.ContainsKey("TITLE") ? dynamicContent["TITLE"] : "Document")
        .Replace("{{SIGNATURE_TEXT}}", dynamicContent.ContainsKey("SIGNATURE_TEXT") ? dynamicContent["SIGNATURE_TEXT"] : "Authorized Signature");

    // Configure the PDF document
    var pdfDocument = new HtmlToPdfDocument
    {
      GlobalSettings =
      {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4
      },
      Objects = 
        {
            new ObjectSettings
            {
                HtmlContent = finalHtml,
                WebSettings = new WebSettings
                {
                    DefaultEncoding = "utf-8"
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

    // Convert the HTML to PDF and return as byte array
    return _converter.Convert(pdfDocument);
  }

  

  public async Task<byte[]> GenerateInvoicePdf()
  {
    string invoiceTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PDF", "InvoiceTemplate.html");

    // Example dynamic content
    var dynamicContent = new Dictionary<string, string>
    {
        { "SENDER_NAME", "Acme Corporation" },
        { "SENDER_ADDRESS", "123 Business Rd, Business City" },
        { "RECIPIENT_NAME", "John Doe" },
        { "RECIPIENT_ADDRESS", "456 Residential St, Hometown" },
        { "INVOICE_ROWS", GenerateInvoiceRows(new List<UserInvoiceItemResponse>
            {
                new UserInvoiceItemResponse { Description = "Web Design", Quantity = 1, UnitPrice = 500 },
                new UserInvoiceItemResponse{ Description = "Hosting", Quantity = 12, UnitPrice = 10 }
            })
        },
        { "TOTAL_AMOUNT", "$620" },
        { "TITLE", "Invoice" },
        { "SIGNATURE_TEXT", "Authorized Signature" }
    };

    return await GeneratePdfWithDynamicContent(invoiceTemplatePath, dynamicContent, "ar"); // Pass "ar" for Arabic
  }
  private string GenerateInvoiceRows(List<UserInvoiceItemResponse> items)
  {
    var sb = new StringBuilder();

    for (int i = 0; i < items.Count; i++)
    {
      var item = items[i];
      sb.Append($@"
            <tr>
                <td>{i + 1}</td>
                <td>{item.Description}</td>
                <td>{item.Quantity}</td>
                <td>{item.UnitPrice:C}</td>
                <td>{(item.Quantity * item.UnitPrice):C}</td>
            </tr>");
    }

    return sb.ToString();
  }

  

}
