using PuppeteerSharp.Media;
using PuppeteerSharp;

public class PdfService : IPdfService
{
  private readonly IUserService _userService;

  public PdfService(IUserService userService)
  {
    _userService = userService;
    //_pdfConverter = pdfConverter;
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


    // Set up BrowserFetcher to download Chromium
    var browserFetcher = new BrowserFetcher();
    await browserFetcher.DownloadAsync();

    // Launch a headless browser
    using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
    using var page = await browser.NewPageAsync();

    // Set the content to be converted to PDF
    await page.SetContentAsync(htmlFileContentForLayout);

    // Convert to PDF
    var pdfBytes = await page.PdfDataAsync(new PdfOptions
    {
      Format = PaperFormat.A4,
      PrintBackground = true,
    });

    return pdfBytes;
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
