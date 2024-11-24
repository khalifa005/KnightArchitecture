namespace KH.Services.Media_s.Contracts;

public interface IPdfService
{
  /// <summary>
  /// Generates a PDF file containing user details based on the provided filters.
  /// </summary>
  /// <param name="param">Filter parameters for the user.</param>
  /// <param name="cancellationToken">Cancellation token for the operation.</param>
  /// <returns>A byte array representing the generated PDF.</returns>
  Task<byte[]> ExportUserDetailsPdfAsync(UserFilterRequest param, CancellationToken cancellationToken);

  /// <summary>
  /// Generates a PDF file using a dynamic HTML template and content.
  /// </summary>
  /// <param name="templatePath">Path to the dynamic HTML template.</param>
  /// <param name="dynamicContent">A dictionary of placeholders and their corresponding values.</param>
  /// <param name="language">Language preference for the content (default is "en").</param>
  /// <returns>A byte array representing the generated PDF.</returns>
  Task<byte[]> GeneratePdfWithDynamicContent(string templatePath, Dictionary<string, string> dynamicContent, string language = "en");

  /// <summary>
  /// Generates a PDF invoice with dynamic content and localization.
  /// </summary>
  /// <param name="language">Language preference for the invoice (default is "en").</param>
  /// <returns>A byte array representing the generated PDF invoice.</returns>
  Task<byte[]> ExportUserInvoicePdf(string language = "en");

  Task<byte[]> GeneratePdfAsync(UserFilterRequest param, CancellationToken cancellationToken);
  Task<byte[]> MergePdfsAsync(List<byte[]> pdfs);
}
