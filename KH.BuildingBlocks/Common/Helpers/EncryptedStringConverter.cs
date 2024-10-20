using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KH.BuildingBlocks.Apis.Helpers;

public class EncryptedStringConverter : ValueConverter<string, string>
{
  public EncryptedStringConverter() : base(
      v => Encrypt(v),
      v => Decrypt(v))
  {
  }

  private static string Encrypt(string plainText)
  {
    // Implement encryption logic
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
  }

  private static string Decrypt(string cipherText)
  {
    // Implement decryption logic
    return Encoding.UTF8.GetString(Convert.FromBase64String(cipherText));
  }
}
