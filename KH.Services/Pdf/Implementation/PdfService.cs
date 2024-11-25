using KH.BuildingBlocks.PDF.Quest;
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
      var placeholdersWithValues = PrepareUseInfoContent(user);

      return await GeneratePdfWithDynamicContent(
        PdfTemplatesConstant.Templates.Customer_Welcome_Template,
        PdfTemplatesConstant.Layouts.Customer_Welcome_Layout,
        placeholdersWithValues,
        param.Language);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error generating user details PDF");
      return Array.Empty<byte>();
    }
  }

  public async Task<byte[]> ExportUserInvoicePdf(string language = "en")
  {
    var placeholdersWithValues = PrepareInvoiceContent(language);

    AddInvoiceTableHeaders(placeholdersWithValues, language);

    return await GeneratePdfWithDynamicContent(PdfTemplatesConstant.Templates.Invoice_Template,
      PdfTemplatesConstant.Layouts.Main_Layout,
      placeholdersWithValues,
      language);
  }


  public async Task<byte[]> GeneratePdfWithDynamicContent(string templateName, string layoutName, Dictionary<string, string> placeholdersWithValues, string language = "en")
  {
    try
    {
      string layoutContent = await LoadTemplateContent(layoutName);
      string templateContent = await LoadTemplateContent(templateName);

      string populatedTemplate = ReplacePlaceholders(templateContent, placeholdersWithValues);
      string finalHtml = ReplacePlaceholders(layoutContent, placeholdersWithValues, language, populatedTemplate);

      return GeneratePdfWithDink(finalHtml, PaperKind.A3);
      //return GeneratePdfFromHtmlWithNReco(finalHtml);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error generating dynamic content PDF");
      return Array.Empty<byte>();
    }
  }

  private byte[] GeneratePdfWithDink(string htmlContent, PaperKind paperKind)
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


  #region QuestPDF
  public async Task<byte[]> GenerateBasicPdfQuestAsync(UserFilterRequest param, CancellationToken cancellationToken)
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


  #endregion

  #region PDFsharp

  public async Task<byte[]> MergePdfsAsync(IEnumerable<IFormFile> formFiles)
  {
    const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
    const int MaxFileCount = 5;

    // Validate the number of files
    if (formFiles == null || !formFiles.Any())
    {
      throw new ArgumentException("No files provided for merging.");
    }

    if (formFiles.Count() > MaxFileCount)
    {
      throw new InvalidOperationException($"A maximum of {MaxFileCount} files can be merged at once.");
    }

    var outputDocument = new PdfDocument();

    foreach (var file in formFiles)
    {
      // Validate file size
      if (file.Length > MaxFileSizeBytes)
      {
        throw new InvalidOperationException($"File {file.FileName} exceeds the size limit of 10 MB.");
      }

      // Validate file type
      if (file.ContentType != "application/pdf")
      {
        throw new InvalidOperationException($"Invalid file format: {file.FileName}. Only PDF files are allowed.");
      }

      if (file.Length == 0)
      {
        throw new InvalidOperationException($"File {file.FileName} is empty.");
      }

      using var stream = file.OpenReadStream();
      PdfDocument inputDocument;

      try
      {
        inputDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error reading PDF file: {file.FileName}.", ex);
      }

      for (int i = 0; i < inputDocument.PageCount; i++)
      {
        var page = inputDocument.Pages[i];
        outputDocument.AddPage(page);
      }
    }

    using var memoryStream = new MemoryStream();
    outputDocument.Save(memoryStream);
    memoryStream.Position = 0; // Reset stream position for reading

    return memoryStream.ToArray();
  }

  public MemoryStream GeneratePdfWithSharp(string title, string content)
  {
    var document = new PdfDocument();
    var page = document.AddPage();
    var gfx = XGraphics.FromPdfPage(page);

    var titleFont = new XFont("Arial", 20, XFontStyle.Bold);
    var contentFont = new XFont("Arial", 12, XFontStyle.Regular);

    gfx.DrawString(title, titleFont, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);
    gfx.DrawString(content, contentFont, XBrushes.Black, new XRect(40, 60, page.Width - 80, page.Height - 100), XStringFormats.TopLeft);

    var memoryStream = new MemoryStream();
    document.Save(memoryStream);
    memoryStream.Position = 0; // Reset stream position for reading

    return memoryStream;
  }


  public MemoryStream GenerateInvoiceWithSharp()
  {
    // Create a new PDF document
    PdfDocument document = new PdfDocument();
    document.Info.Title = "Invoice";

    // Add a page
    PdfPage page = document.AddPage();
    XGraphics gfx = XGraphics.FromPdfPage(page);

    // Set fonts
    XFont headerFont = new XFont("Arial", 18, XFontStyle.Bold);
    XFont subHeaderFont = new XFont("Arial", 14, XFontStyle.Regular);
    XFont textFont = new XFont("Arial", 12, XFontStyle.Regular);

    // Draw header
    gfx.DrawString("INVOICE", headerFont, XBrushes.Black, new XPoint(page.Width / 2, 40), XStringFormats.TopCenter);

    // Company Info
    gfx.DrawString("Your Company Name", subHeaderFont, XBrushes.Black, 40, 80);
    gfx.DrawString("1234 Example Street, Riyadh, SA", textFont, XBrushes.Black, 40, 100);
    gfx.DrawString("Phone: +966 123 456 789", textFont, XBrushes.Black, 40, 120);
    gfx.DrawString("Email: info@yourcompany.com", textFont, XBrushes.Black, 40, 140);

    // Customer Info
    gfx.DrawString("Billed To:", subHeaderFont, XBrushes.Black, 40, 180);
    gfx.DrawString("John Doe", textFont, XBrushes.Black, 40, 200);
    gfx.DrawString("5678 Sample Road, Medina, SA", textFont, XBrushes.Black, 40, 220);

    // Invoice Details
    gfx.DrawString("Invoice #: 12345", textFont, XBrushes.Black, 40, 260);
    gfx.DrawString("Date: " + System.DateTime.Now.ToString("yyyy-MM-dd"), textFont, XBrushes.Black, 40, 280);

    // Table Header
    gfx.DrawString("Description", textFont, XBrushes.Black, 40, 320);
    gfx.DrawString("Quantity", textFont, XBrushes.Black, 300, 320);
    gfx.DrawString("Unit Price", textFont, XBrushes.Black, 400, 320);
    gfx.DrawString("Total", textFont, XBrushes.Black, 500, 320);

    gfx.DrawLine(XPens.Black, 40, 340, page.Width - 40, 340);

    // Table Content (Static data for demonstration)
    gfx.DrawString("Sample Product 1", textFont, XBrushes.Black, 40, 360);
    gfx.DrawString("2", textFont, XBrushes.Black, 300, 360);
    gfx.DrawString("500.00 SAR", textFont, XBrushes.Black, 400, 360);
    gfx.DrawString("1000.00 SAR", textFont, XBrushes.Black, 500, 360);

    gfx.DrawString("Sample Product 2", textFont, XBrushes.Black, 40, 380);
    gfx.DrawString("1", textFont, XBrushes.Black, 300, 380);
    gfx.DrawString("250.00 SAR", textFont, XBrushes.Black, 400, 380);
    gfx.DrawString("250.00 SAR", textFont, XBrushes.Black, 500, 380);

    // Total
    gfx.DrawLine(XPens.Black, 40, 400, page.Width - 40, 400);
    gfx.DrawString("Grand Total:", subHeaderFont, XBrushes.Black, 400, 420);
    gfx.DrawString("1250.00 SAR", subHeaderFont, XBrushes.Black, 500, 420);

    // Save to MemoryStream
    var memoryStream = new MemoryStream();
    document.Save(memoryStream);
    memoryStream.Position = 0;

    return memoryStream;
  }

  #endregion

  #region NReco
  public byte[] GeneratePdfFromHtmlWithNReco(string htmlContent = "")
  {

    if (string.IsNullOrWhiteSpace(htmlContent))
    {
      htmlContent = "<h1>Invoice</h1><p>This is a sample invoice generated as a PDF.</p>";
      //throw new ArgumentException("HTML content cannot be empty.");
    }

    var htmlToPdf = new HtmlToPdfConverter
    {
      // Optional: Configure the PDF options
      Size = NReco.PdfGenerator.PageSize.A3,
      Orientation = PageOrientation.Portrait,
      CustomWkHtmlArgs = "--no-outline"
    };

    return htmlToPdf.GeneratePdf(htmlContent);
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
