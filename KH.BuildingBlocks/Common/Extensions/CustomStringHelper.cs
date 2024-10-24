namespace KH.BuildingBlocks.Apis.Extentions;

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

}

