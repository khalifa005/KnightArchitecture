using System.ComponentModel.DataAnnotations;

namespace KH.BuildingBlocks.Settings;

public class FileSettings
{
  public long MaxFileSizeInBytes { get; set; }
  public string[] AllowedExtensions { get; set; }
  public string ServerPartition { get; set; }
  //foldeer  to have flexibility to change the path of the folder
  [Required]  // This ensures the DirectoryPath property is not null or empty
  [StringLength(100, MinimumLength = 5)]  // This restricts the length of the DirectoryPath
  public string FolderName { get; set; }
  public string VirusTotalApiKey { get; set; }

}
