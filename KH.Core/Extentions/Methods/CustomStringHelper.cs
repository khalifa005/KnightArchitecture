namespace KH.Helper.Extentions.Methods
{
  public class CustomStringHelper
  {
    public static string Base64Decode(string base64EncodedData)
    {
      byte[] bytes = Convert.FromBase64String(base64EncodedData);
      return Encoding.UTF8.GetString(bytes);
    }

    public static string Base64Encode(string plainText) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

    public static DateTime? ConvertHijriToGregorian(string hijriDate)
    {
      DateTime time;
      DateTime? nullable;
      string[] formats = new string[] { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy" };
      if (DateTime.TryParseExact(hijriDate, formats, new CultureInfo("ar-SA").DateTimeFormat, DateTimeStyles.AllowInnerWhite, out time))
      {
        nullable = new DateTime?(time);
      }
      else
      {
        nullable = null;
      }
      return nullable;
    }

    public static string Decrypt(string cipherString)
    {
      byte[] inputBuffer = Convert.FromBase64String(cipherString);
      string s = "asdfewrewqrss323";
      TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider
      {
        Key = Encoding.UTF8.GetBytes(s),
        Mode = CipherMode.ECB,
        Padding = PaddingMode.PKCS7
      };
      byte[] bytes = provider.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
      provider.Clear();
      return Encoding.UTF8.GetString(bytes);
    }

    public static string Encrypt(string toEncrypt)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
      string s = "asdfewrewqrss323";
      TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider
      {
        Key = Encoding.UTF8.GetBytes(s),
        Mode = CipherMode.ECB,
        Padding = PaddingMode.PKCS7
      };
      byte[] inArray = provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
      provider.Clear();
      return Convert.ToBase64String(inArray, 0, inArray.Length);
    }

    public static string MD5Hash(string text)
    {
      MD5 md = new MD5CryptoServiceProvider();
      md.ComputeHash(Encoding.ASCII.GetBytes(text));
      byte[] hash = md.Hash;
      StringBuilder builder = new StringBuilder();
      for (int i = 0; i < hash.Length; i++)
      {
        builder.Append(hash[i].ToString("x2"));
      }
      return builder.ToString();
    }

    public static string RemoveTrailingSpace(string input) =>
        !string.IsNullOrEmpty(input) ? input.ToString().Trim() : input;

    public static string SubString(string input, int length) =>
        !string.IsNullOrEmpty(input) ? input.Length > length ? input.Substring(0, length) : input : input;

    public static string ToCurrency(string amount) =>
        "SAR " + amount;

    public static string ToFormattedString(string input)
    {
      string str = "-";
      if (!string.IsNullOrEmpty(input))
      {
        str = input;
      }
      return str;
    }

    public static string ToSentenceCase(string str)
    {
      TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
      str = Regex.Replace(str, "[^a-zA-Z0-9 ]+", " ");
      return textInfo.ToTitleCase(str);
    }

    public static string TrimString(string input, int maxLength) =>
        !string.IsNullOrEmpty(input) ? input.Length <= maxLength ? input : input.Substring(0, maxLength - 1) : input;


    static void computedPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      }
    }

    static bool verifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512())
      {
        var computHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computHash.Length; i++)
        {
          if (passwordHash[i] != passwordSalt[i])
            return false;
        }
      }

      return true;
    }
  }
}

