namespace KH.BuildingBlocks.Apis.Extentions;

public static class Base64Helper
{
  public static string Base64Decode(string base64EncodedData)
  {
    byte[] bytes = Convert.FromBase64String(base64EncodedData);
    return Encoding.UTF8.GetString(bytes);
  }

  public static string Base64Encode(string plainText) =>
      Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
}

