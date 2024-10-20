using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KH.BuildingBlocks.Apis.Helpers;

public class SymmetricStringConverter : ValueConverter<string, string>
{
  private static readonly byte[] Key = Encoding.UTF8.GetBytes("your-encryption-key");

  public SymmetricStringConverter() : base(
      v => Encrypt(v),
      v => Decrypt(v))
  {
  }

  private static string Encrypt(string plainText)
  {
    using (var aes = Aes.Create())
    {
      aes.Key = Key;
      aes.GenerateIV();
      var iv = aes.IV;
      var encryptor = aes.CreateEncryptor(aes.Key, iv);

      using (var ms = new MemoryStream())
      {
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
          sw.Write(plainText);
        }

        var encrypted = ms.ToArray();
        return Convert.ToBase64String(iv.Concat(encrypted).ToArray());
      }
    }
  }

  private static string Decrypt(string cipherText)
  {
    var fullCipher = Convert.FromBase64String(cipherText);
    using (var aes = Aes.Create())
    {
      aes.Key = Key;
      var iv = fullCipher.Take(aes.BlockSize / 8).ToArray();
      var cipher = fullCipher.Skip(aes.BlockSize / 8).ToArray();
      aes.IV = iv;

      var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
      using (var ms = new MemoryStream(cipher))
      using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
      using (var sr = new StreamReader(cs))
      {
        return sr.ReadToEnd();
      }
    }
  }
}
