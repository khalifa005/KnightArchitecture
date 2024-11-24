using KH.BuildingBlocks.Apis.Services;
using System.Globalization;
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

  public async Task<byte[]> GeneratePdf(string htmlTemplateName, string htmlLayoutName)
  {
    //this will handle all PDF export just pass the HTML content and it will take care 

    string filePathForTemplateLayout = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PDF", "BasicHtmlTemplate.html");
    string htmlFileContentForLayout = await File.ReadAllTextAsync(filePathForTemplateLayout);


    if (string.IsNullOrEmpty(htmlFileContentForLayout))
      throw new ArgumentException("HTML content cannot be null or empty.", nameof(htmlTemplateName));

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

  public async Task<byte[]> GeneratePdfWithDynamicContent(
     string templatePath,
     Dictionary<string, string> dynamicContent,
     string language = "en")
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

    // Determine language direction and CSS class
    string languageDirection = language == "ar" ? "rtl" : "ltr";
    string languageClass = language == "ar" ? "arabic" : "";

    // Insert populated content and language direction into layout
    string finalHtml = layoutContent
        .Replace("{{CONTENT_PLACEHOLDER}}", templateContent)
        .Replace("{{LANGUAGE_DIRECTION}}", languageDirection)
        .Replace("{{LANGUAGE_CLASS}}", languageClass)
        .Replace("{{TITLE}}", dynamicContent.ContainsKey("TITLE") ? dynamicContent["TITLE"] : "Document")
        .Replace("{{SIGNATURE_TEXT}}", dynamicContent.ContainsKey("SIGNATURE_TEXT") ? dynamicContent["SIGNATURE_TEXT"] : "Authorized Signature")
        .Replace("{{DATE}}", DateTime.Now.ToString("yyyy-MM-dd"))
        .Replace("{{PAGE}}", newValue: "1") // Placeholder for page numbering
        .Replace("{{TO_PAGE}}", "1"); // Placeholder for total pages

    // Configure the PDF document
    var pdfDocument = new HtmlToPdfDocument
    {
      GlobalSettings = 
      {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4,
        DocumentTitle = dynamicContent["TITLE"]
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

  public async Task<byte[]> GenerateInvoicePdf(string language = "en")
  {
    string invoiceTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PDF", "InvoiceTemplate.html");

    // Define dynamic content inline using language condition
    var dynamicContent = new Dictionary<string, string>
    {
        { "SENDER_NAME", language == "ar" ? "شركة المثال" : "Acme Corporation" },
        { "SENDER_ADDRESS", language == "ar" ? "123 شارع الأعمال، المدينة التجارية" : "123 Business Rd, Business City" },
        { "RECIPIENT_NAME", language == "ar" ? "محمد أحمد" : "John Doe" },
        { "RECIPIENT_ADDRESS", language == "ar" ? "456 شارع السكن، المدينة" : "456 Residential St, Hometown" },
        { "INVOICE_ROWS", GenerateInvoiceRows(new List<UserInvoiceItemResponse>
            {
                new UserInvoiceItemResponse { Description = language == "ar" ? "تصميم ويب" : "Web Design", Quantity = 1, UnitPrice = 500 },
                new UserInvoiceItemResponse { Description = language == "ar" ? "استضافة" : "Hosting", Quantity = 12, UnitPrice = 10 }
            }, language)
        },
        { "TOTAL_AMOUNT", language == "ar" ? "620.00 ر.س" : "$620.00" },
        { "TOTAL_AMOUNT_TITLE", language == "ar" ? "الاجمالي" : "Total amount" },
        { "TITLE", language == "ar" ? "فاتورة" : "Invoice" },
        { "TO", language == "ar" ? "المصدره الى" : "To" },
        { "SIGNATURE_TEXT", language == "ar" ? "توقيع المسؤول" : "Authorized Signature" }
    };

    // Add table headers dynamically
    AddTableHeaders(dynamicContent, language);

    // Generate the PDF with the selected language content
    return await GeneratePdfWithDynamicContent(invoiceTemplatePath, dynamicContent, language);
  }

  private string GenerateInvoiceRows(List<UserInvoiceItemResponse> items, string language)
  {
    var culture = language == "ar" ? new CultureInfo("ar-SA") : CultureInfo.InvariantCulture;
    var sb = new StringBuilder();

    for (int i = 0; i < items.Count; i++)
    {
      var item = items[i];
      sb.Append($@"
            <tr>
                <td>{(i + 1).ToString(culture)}</td>
                <td>{item.Description}</td>
                <td>{item.Quantity.ToString(culture)}</td>
                <td>{item.UnitPrice.ToString("C", culture)}</td>
                <td>{(item.Quantity * item.UnitPrice).ToString("C", culture)}</td>
            </tr>");
    }

    return sb.ToString();
  }

  private void AddTableHeaders(Dictionary<string, string> dynamicContent, string language)
  {
    dynamicContent.Add("TABLE_HEADER_DESCRIPTION", language == "ar" ? "الوصف" : "Description");
    dynamicContent.Add("TABLE_HEADER_QUANTITY", language == "ar" ? "الكمية" : "Quantity");
    dynamicContent.Add("TABLE_HEADER_UNIT_PRICE", language == "ar" ? "سعر الوحدة" : "Unit Price");
    dynamicContent.Add("TABLE_HEADER_TOTAL", language == "ar" ? "الإجمالي" : "Total");
  }


}
