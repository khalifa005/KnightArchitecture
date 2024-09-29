
using FluentValidation;
using RestSharp;



namespace KH.BuildingBlocks.Extentions.Files;

public class FileValidator : AbstractValidator<IFormFile>
{
  private readonly string _virusTotalApiKey;
  private readonly long _maxFileSize; // Max file size in bytes
  private readonly string[] _allowedFileTypes;

  public FileValidator(long maxFileSize, string[] allowedFileTypes)
  {
    _maxFileSize = maxFileSize;
    _allowedFileTypes = allowedFileTypes;

    RuleFor(file => file)
        .NotNull().WithMessage("File is required.")
        .Must(file => file.Length > 0).WithMessage("File cannot be empty.")
        .Must(BeValidFileType).WithMessage($"Invalid file type. Allowed types: {string.Join(", ", allowedFileTypes)}")
        .Must(BeValidFileSize).WithMessage($"File size must not exceed {_maxFileSize / (1024 * 1024)} MB.");
  }

  private bool BeValidFileType(IFormFile file)
  {
    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
    return Array.Exists(_allowedFileTypes, type => type.Equals(extension, StringComparison.OrdinalIgnoreCase));
  }

  private bool BeValidFileSize(IFormFile file)
  {
    return file.Length <= _maxFileSize;
  }
  public async Task ScanForMalwareAsync(Stream fileStream, string fileName)
  {
    var client = new RestClient("https://www.virustotal.com/vtapi/v2/file/scan");

    // Initialize RestRequest for POST request
    var request = new RestRequest
    {
      Method = Method.Post
    };

    // Add headers and file data
    request.AddHeader("x-apikey", _virusTotalApiKey);
    request.AddFile("file", ReadFully(fileStream), fileName);

    // Execute the request asynchronously
    var response = await client.ExecuteAsync(request);

    // Check if the request was successful
    if (!response.IsSuccessful)
    {
      throw new InvalidOperationException($"Failed to scan file for malware. Error: {response.ErrorMessage}");
    }

    // Handle the response (parse response JSON or process the result)
    // For simple logging or handling
    Console.WriteLine(response.Content);
  }
  private static byte[] ReadFully(Stream input)
  {
    using (MemoryStream ms = new MemoryStream())
    {
      input.CopyTo(ms);
      return ms.ToArray();
    }
  }
}
