
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace CA.Application.Helpers
{
  // how to use 
  //private readonly FileSettings _fileSettings;
  //public FileService(IOptions<FileSettings> options)
  //{
  //  _fileSettings = options.Value;
  //}

  public class FileSettings
  {
    public long MaxFileSizeInBytes { get; set; }
    public string[] AllowedExtensions { get; set; }

    [Required]  // This ensures the DirectoryPath property is not null or empty
    [StringLength(100, MinimumLength = 5)]  // This restricts the length of the DirectoryPath
    public string UploadPath { get; set; }
    //foldeer  to have flexibility to change the path of the folder
    public string FolderName { get; set; }
    public string VirusTotalApiKey { get; set; }
    
  }
}
