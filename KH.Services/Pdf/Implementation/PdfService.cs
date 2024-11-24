using KH.BuildingBlocks.Apis.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Globalization;
using System.Text;
using static KH.Dto.Models.UserDto.Response.UserRoleResponse;

namespace KH.Services.Pdf.Implementation;

public class PdfService : IPdfService
{
  private readonly IUserQueryService _userService;
  private readonly IConverter _converter;
  private readonly ILogger<PdfService> _logger;

  public PdfService(IUserQueryService userService, IConverter converter, ILogger<PdfService> logger)
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

      var userResponse = await _userService.GetAsync(param.Id!.Value, cancellationToken);

      if (userResponse.StatusCode != StatusCodes.Status200OK || userResponse.Data == null)
      {
        _logger.LogWarning($"Invalid user data for ID {param.Id}");
        return Array.Empty<byte>();
      }

      var user = userResponse.Data;
      var placeholders = PrepareUseInfoContent(user);

      string htmlBody = PopulateTemplateFromFile("CustomerPdfTemplateBody.html", placeholders);
      string finalHtml = PopulateTemplateFromFile("CustomerPdfLayout.html", new Dictionary<string, string> { { "Htmlbody", htmlBody } });

      return GeneratePdf(finalHtml, PaperKind.A3Extra);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error generating user details PDF");
      return Array.Empty<byte>();
    }
  }

  public async Task<byte[]> ExportUserInvoicePdf(string language = "en")
  {
    var invoiceTemplatePath = GetTemplatePath("InvoiceTemplate.html");

    var dynamicContent = PrepareInvoiceContent(language);
    AddInvoiceTableHeaders(dynamicContent, language);

    return await GeneratePdfWithDynamicContent(invoiceTemplatePath, dynamicContent, language);
  }


  public async Task<byte[]> GeneratePdfWithDynamicContent(string templatePath, Dictionary<string, string> dynamicContent, string language = "en")
  {
    try
    {
      string layoutContent = await LoadTemplateContent("PdfLayoutTemplate.html");
      string templateContent = await LoadTemplateContent(templatePath);

      string populatedTemplate = ReplacePlaceholders(templateContent, dynamicContent);
      string finalHtml = ReplacePlaceholders(layoutContent, dynamicContent, language, populatedTemplate);

      return GeneratePdf(finalHtml, PaperKind.A4);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error generating dynamic content PDF");
      return Array.Empty<byte>();
    }
  }

  private byte[] GeneratePdf(string htmlContent, PaperKind paperKind)
  {
    var pdfDocument = new HtmlToPdfDocument
    {
      GlobalSettings =
      {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = paperKind,
        Out = null
      },
      Objects =
            {
                new ObjectSettings
                {
                    HtmlContent = htmlContent,
                    WebSettings = new WebSettings { DefaultEncoding = "utf-8" },
                    HeaderSettings = new HeaderSettings
                    {
                        FontSize = 10,
                        //Right = "Page [page] of [toPage]",
                        //Line = true
                    },
                    FooterSettings = new FooterSettings
                    {
                        FontSize = 10,
                        Center = "Generated on [date]"
                    }
                }
            }
    };

    return _converter.Convert(pdfDocument);
  }


  #region DemoQuestPDF
  // Generate a PDF with user information
  public async Task<byte[]> GeneratePdfAsync(UserFilterRequest param, CancellationToken cancellationToken)
  {
    var userResponse = await _userService.GetAsync(param.Id!.Value, cancellationToken);

    if (userResponse.StatusCode != StatusCodes.Status200OK || userResponse.Data == null)
    {
      _logger.LogWarning($"Invalid user data for ID {param.Id}");
      return Array.Empty<byte>();
    }

    var user = userResponse.Data;

    var pdfBytes = Document.Create(container =>
    {
      container.Page(page =>
      {
        page.Margin(50);
        page.Size(PageSizes.A4);
        page.Background(Colors.White);

        // Header
        page.Header().Height(80).Background(Colors.Blue.Medium).AlignMiddle().AlignCenter()
            .Text("Welcome Document")
            .FontSize(24).FontColor(Colors.White).Bold();

        // Content
        page.Content().Column(column =>
        {
          column.Spacing(20);

          // Personal Details
          column.Item().Text($"Hello, {user.FirstName}!").FontSize(20).Bold().FontColor(Colors.Blue.Medium);
          column.Item().Text($"Email: {user.Email}").FontSize(16).FontColor(Colors.Grey.Darken2);

          // Roles Section
          column.Item().Text("Your Roles:").FontSize(18).Bold().FontColor(Colors.Grey.Darken3);
          foreach (var role in user.UserRoles.Select(r => r.Role.NameEn))
          {
            column.Item().Text($"- {role}").FontSize(14).FontColor(Colors.Blue.Darken2);
          }

          // Footer Note
          column.Item().PaddingTop(20).Text("Thank you for being part of our service!")
              .FontSize(14).Italic().FontColor(Colors.Grey.Darken1);
        });

        // Footer
        page.Footer().Height(50).AlignMiddle().AlignRight().Text(text =>
        {
          text.Span("Generated with QuestPDF").FontColor(Colors.Grey.Darken1).FontSize(10);
          text.Span(" | Page ").FontSize(10);
          text.CurrentPageNumber().FontSize(10);
        });
      });
    }).GeneratePdf();

    return await Task.FromResult(pdfBytes);
  }

  public async Task<byte[]> GenerateInvoicePdfWithQuestAsync()
  {
    var model = InvoiceDocumentDataSource.GetInvoiceDetails();

    // Generate PDF as byte array
    var document = new InvoiceDocument(model);
    var pdfBytes = document.GeneratePdf();
    return pdfBytes;
  }



  // Merge multiple PDFs into one
  public async Task<byte[]> MergePdfsAsync(List<byte[]> pdfs)
  {
    var mergedDocument = Document.Create(container =>
    {
      container.Page(page =>
      {
        page.Size(PageSizes.A4);
        page.Margin(0);

        page.Content().Column(column =>
        {
          foreach (var pdf in pdfs)
          {
            column.Item().Element(container =>
            {
              container.ShowEntire().Image(pdf); // Render each PDF as an image
            });

            column.Item().PageBreak(); // Ensure separation between PDFs
          }
        });
      });
    }).GeneratePdf();

    return await Task.FromResult(mergedDocument);
  }
  #endregion

  #region HelperMethods

  private string PopulateTemplateFromFile(string fileName, Dictionary<string, string> placeholders)
  {
    var filePath = GetTemplatePath(fileName);
    return ReplacePlaceholders(File.ReadAllText(filePath), placeholders);
  }

  private async Task<string> LoadTemplateContent(string fileName)
  {
    var filePath = GetTemplatePath(fileName);
    return await File.ReadAllTextAsync(filePath);
  }

  private string ReplacePlaceholders(string template, Dictionary<string, string> placeholders, string language = "en", string contentPlaceholder = null)
  {
    foreach (var placeholder in placeholders)
    {
      //template = template.Replace($"{{{placeholder.Key}}}", placeholder.Value);
      template = template.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);

    }

    if (!string.IsNullOrEmpty(contentPlaceholder))
    {
      template = template.Replace("{{CONTENT_PLACEHOLDER}}", contentPlaceholder);
    }

    template = template
        .Replace("{{LANGUAGE_DIRECTION}}", language == "ar" ? "rtl" : "ltr")
        .Replace("{{LANGUAGE_CLASS}}", language == "ar" ? "arabic" : "");

    return template;
  }

  private string GetTemplatePath(string fileName)
  {
    return Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PDF", fileName);
  }

  #endregion

  #region UnrelatedMethodJustForShowingADirectDemo
  private Dictionary<string, string> PrepareInvoiceContent(string language)
  {
    var rows = GenerateInvoiceRowsData(new List<UserInvoiceItemResponse>
        {
            new UserInvoiceItemResponse { Description = language == "ar" ? "تصميم ويب" : "Web Design", Quantity = 1, UnitPrice = 500 },
            new UserInvoiceItemResponse { Description = language == "ar" ? "استضافة" : "Hosting", Quantity = 12, UnitPrice = 10 }
        }, language);

    return new Dictionary<string, string>
        {
            { "SENDER_NAME", language == "ar" ? "شركة المثال" : "Acme Corporation" },
            { "SENDER_ADDRESS", language == "ar" ? "123 شارع الأعمال" : "123 Business St" },
            { "RECIPIENT_NAME", language == "ar" ? "محمد أحمد" : "John Doe" },
            { "RECIPIENT_ADDRESS", language == "ar" ? "456 شارع السكن" : "456 Residential St" },
            { "INVOICE_ROWS", rows },
            { "TOTAL_AMOUNT", language == "ar" ? "620.00 ر.س" : "$620.00" },
            { "TOTAL_AMOUNT_TITLE", language == "ar" ? "الاجمالي" : "Total Amount" },
            { "TITLE", language == "ar" ? "فاتورة" : "Invoice" },
            { "SIGNATURE_TEXT", language == "ar" ? "توقيع المسؤول" : "Authorized Signature" }
        };
  }

  private string GenerateInvoiceRowsData(List<UserInvoiceItemResponse> items, string language)
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

  private void AddInvoiceTableHeaders(Dictionary<string, string> dynamicContent, string language)
  {
    dynamicContent.Add("TABLE_HEADER_DESCRIPTION", language == "ar" ? "الوصف" : "Description");
    dynamicContent.Add("TABLE_HEADER_QUANTITY", language == "ar" ? "الكمية" : "Quantity");
    dynamicContent.Add("TABLE_HEADER_UNIT_PRICE", language == "ar" ? "سعر الوحدة" : "Unit Price");
    dynamicContent.Add("TABLE_HEADER_TOTAL", language == "ar" ? "الإجمالي" : "Total");
  }

  private Dictionary<string, string> PrepareUseInfoContent(UserDetailsResponse user)
  {
    return new Dictionary<string, string>
        {
            { "FullName", user.FullName },
            { "Email", user.Email },
            { "MobileNumber", user.MobileNumber },
            { "Username", user.Username },
            { "BirthDate", user.BirthDate?.ToString("dd/MM/yyyy") ?? "N/A" },
            { "OtherDetails", $"Roles: {string.Join(", ", user.UserRoles.Select(r => r.Role.NameEn))}" }
        };
  }


  #endregion


}
