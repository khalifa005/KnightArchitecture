using System.ComponentModel;

namespace KH.BuildingBlocks.Files.Enums;

public enum UploadType : byte
{
  [Description(@"Images\Products")]
  Product,

  [Description(@"Images\ProfilePictures")]
  ProfilePicture,

  [Description(@"Documents")]
  Document
}

