using KH.BuildingBlocks.Files.Responses;
using KH.BuildingBlocks.Files.Validation;
using KH.BuildingBlocks.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace KH.BuildingBlocks.Files.Services;

public class FileManagerService
{
  private readonly FileSettings _fileSettings;
  public FileManagerService(ILogger<FileManagerService> logger,
    IOptions<FileSettings> fileSettings)
  {
    _fileSettings = fileSettings.Value;
  }

  public async Task<FileResponse> Upload(IFormFile file, string modelWithModelId)
  {
    try
    {
      if (_fileSettings == null || file is null || file.Length <= 0)
      {
        return new FileResponse() { IsValid = false, FilePath = "" };
      }

      var fileValidator = new FileValidator(_fileSettings.MaxFileSizeInBytes, _fileSettings.AllowedExtensions);
      var validationResult = await fileValidator.ValidateAsync(file);

      if (!validationResult.IsValid)
      {
        return new FileResponse() { IsValid = false, Message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)) };
      }

      //var uploadPath = ;
      var folder_full_path = string.Concat(_fileSettings.ServerPartition, _fileSettings.FolderName);
      if (!Directory.Exists(folder_full_path))
      {
        Directory.CreateDirectory(folder_full_path);
      }

      var modelDirectory = Path.Combine(folder_full_path, modelWithModelId);
      if (!Directory.Exists(modelDirectory))
      {
        Directory.CreateDirectory(modelDirectory);
      }

      var originalFileNameX = GenerateUniqueFileName(file.FileName.Trim(' '));
      var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
      var generatedFileName = $"{DateTime.Now:MM-dd-yyyy-h-mm}-{originalFileName}";


      var fullPath = Path.Combine(modelDirectory, generatedFileName);
      var dbPath = Path.Combine(_fileSettings.FolderName, modelWithModelId, generatedFileName);

      using (var stream = new FileStream(fullPath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      string fileExtention = Path.GetExtension(fullPath);

      return new FileResponse()
      {
        IsValid = true,
        GeneratedFileName = generatedFileName,
        OrignalFileName = originalFileName,
        FilePath = dbPath,
        ContentType = file.ContentType,
        FileExtention = fileExtention
      };

    }
    catch (Exception ex)
    {
      return new FileResponse() { IsValid = false, FilePath = "", Message = ex.Message };
    }
  }

  public async Task<IEnumerable<FileResponse>> UploadMultiple(IFormFileCollection files, string modelWithModelId)
  {
    try
    {
      var filesResponse = new List<FileResponse>();

      if (_fileSettings == null || files is null || files.Count < 1)
      {
        return filesResponse;
      }

      var folder_full_path = string.Concat(_fileSettings.ServerPartition, _fileSettings.FolderName);

      if (!Directory.Exists(folder_full_path))
      {
        Directory.CreateDirectory(folder_full_path);
      }

      var modelDirectory = Path.Combine(folder_full_path, modelWithModelId);
      if (!Directory.Exists(modelDirectory))
      {
        Directory.CreateDirectory(modelDirectory);
      }


      if (files.Any(f => f.Length == 0))
      {
        return filesResponse;
      }

      foreach (var file in files)
      {
        if (file is null || file.Length <= 0)
        {
          filesResponse.Add(new FileResponse() { IsValid = false, FilePath = "" });
          return filesResponse;
        }

        var fileValidator = new FileValidator(_fileSettings.MaxFileSizeInBytes, _fileSettings.AllowedExtensions);
        var validationResult = await fileValidator.ValidateAsync(file);

        if (!validationResult.IsValid)
        {
          filesResponse.Add(new FileResponse() { IsValid = false, Message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)) });
          return filesResponse;

        }

        var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

        var generatedFileName = $"{DateTime.Now:MM-dd-yyyy-h-mm}-{originalFileName}";

        var fullPath = Path.Combine(modelDirectory, generatedFileName);

        var dbPath = Path.Combine(_fileSettings.FolderName, modelDirectory, generatedFileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
          file.CopyTo(stream);
        }

        string fileExtention = Path.GetExtension(fullPath);

        var uploadedFile = new FileResponse()
        {
          IsValid = true,
          GeneratedFileName = generatedFileName,
          OrignalFileName = originalFileName,
          FilePath = dbPath,
          FileExtention = fileExtention
        };

        filesResponse.Add(uploadedFile);
      }

      return filesResponse;

    }
    catch (Exception ex)
    {
      return new List<FileResponse>();
    }
  }

  public async Task<FileResponse> Download(string filePath)
  {
    try
    {
      if (!IsValidFile(filePath))
      {
        return new FileResponse() { IsValid = false, Message = "empty-path" };
      }

      var filePhysicalPath = string.Concat(_fileSettings.ServerPartition, filePath);

      var fileName = Path.GetFileName(filePath);


      var content = await File.ReadAllBytesAsync(filePhysicalPath);

      new FileExtensionContentTypeProvider()
          .TryGetContentType(fileName, out string contentType);

      var fileContentResult = new FileContentResult(content, contentType);

      return new FileResponse()
      {
        IsValid = true,
        FileContentResult = fileContentResult,
        GeneratedFileName = fileName
      };
    }
    catch (Exception ex)
    {
      return new FileResponse() { IsValid = false, Message = ex.Message };

    }
  }

  public FileResponse Delete(string filePath)
  {
    //we will keep the file but this in case we need to remove
    try
    {
      if (IsValidFile(filePath))
      {
        throw new Exception("empty-file-path");
      }

      var pathToDelete = Path.Combine(Directory.GetCurrentDirectory(), filePath);

      if (File.Exists(pathToDelete))
      {
        File.Delete(pathToDelete);

        return new FileResponse() { IsDeleted = true };
      }

      return new FileResponse() { IsDeleted = false };
    }
    catch (Exception ex)
    {
      return new FileResponse() { IsValid = false, Message = ex.Message };
    }
  }

  public async Task<byte[]> ConvertFormFileToBytesAsync(IFormFile file)
  {
    using var memoryStream = new MemoryStream();
    await file.CopyToAsync(memoryStream);
    return memoryStream.ToArray();
  }

  private bool IsPhoto(string fileName)
  {
    return fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
        || fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
        || fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase);
  }

  private bool IsValidFile(string filePath)
  {
    if (_fileSettings == null)
    {
      return false;
    }

    //if (string.IsNullOrEmpty(_fileSettings.UploadPath))
    //{
    //  return false;
    //}

    if (string.IsNullOrEmpty(filePath))
    {
      return false;
    }

    //if (!File.Exists(filePath))
    //{
    //    return false;
    //}

    return true;
  }

  private string SanitizeFileName(string fileName)
  {
    // Remove any invalid characters for file paths
    foreach (char c in Path.GetInvalidFileNameChars())
    {
      fileName = fileName.Replace(c, '_');
    }

    return fileName;
  }

  private string GenerateUniqueFileName(string originalFileName)
  {
    var sanitizedFileName = SanitizeFileName(originalFileName);
    var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
    var hash = GetFileHash(sanitizedFileName);

    return $"{timestamp}_{hash}_{sanitizedFileName}";
  }

  private string GetFileHash(string fileName)
  {
    using (var sha256 = SHA256.Create())
    {
      byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(fileName));
      return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant().Substring(0, 8);  // First 8 characters
    }
  }


}
