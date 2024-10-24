namespace KH.BuildingBlocks.Apis.Helpers;

public class CryptoRandomGenerator
{
  #region Constants
  private const int INT_SIZE = 4;  // Corrected INT_SIZE to 4 bytes for int32
  private const int INT64_SIZE = 8;
  #endregion

  #region Fields
  private static readonly System.Security.Cryptography.RandomNumberGenerator _random;
  #endregion

  #region Constructor
  static CryptoRandomGenerator()
  {
    _random = System.Security.Cryptography.RandomNumberGenerator.Create();
  }
  #endregion

  #region Random Int32
  /// <summary>
  /// Get the next random integer
  /// </summary>
  /// <returns>Random [Int32]</returns>
  public static int Next()
  {
    byte[] data = new byte[INT_SIZE];
    _random.GetBytes(data);
    return BitConverter.ToInt32(data, 0);
  }

  /// <summary>
  /// Get the next random integer to a maximum value
  /// </summary>
  /// <param name="maxValue">Maximum value</param>
  /// <returns>Random [Int32]</returns>
  public static int Next(int maxValue)
  {
    int result;
    do
    {
      result = Next();
    } while (result > maxValue || result < 0);  // Ensuring positive integers
    return result;
  }

  /// <summary>
  /// Get the next random integer for Ticket ID generation
  /// </summary>
  /// <returns>Random [Int32]</returns>
  public static int TicketIdGenerator()
  {
    int result;
    do
    {
      result = Next();
    } while (result < 0);  // Ensuring positive integers
    return result;
  }

  /// <summary>
  /// Generate One Time Password (OTP)
  /// </summary>
  /// <param name="otpLength"></param>
  /// <param name="maxValue"></param>
  /// <returns></returns>
  public static string GenerateOTP(int otpLength = 4, int maxValue = 9)
  {
    StringBuilder otp = new StringBuilder();
    for (int i = 0; i < otpLength; i++)
    {
      otp.Append(Next(maxValue));
    }
    return otp.ToString();
  }
  #endregion

  #region Random UInt32
  /// <summary>
  /// Get the next random unsigned integer
  /// </summary>
  /// <returns>Random [UInt32]</returns>
  public static uint NextUInt()
  {
    byte[] data = new byte[INT_SIZE];
    _random.GetBytes(data);
    return BitConverter.ToUInt32(data, 0);
  }

  /// <summary>
  /// Get the next random unsigned integer to a maximum value
  /// </summary>
  /// <param name="maxValue">Maximum value</param>
  /// <returns>Random [UInt32]</returns>
  public static uint NextUInt(uint maxValue)
  {
    uint result;
    do
    {
      result = NextUInt();
    } while (result > maxValue);
    return result;
  }
  #endregion

  #region Random Int64
  /// <summary>
  /// Get the next random long integer
  /// </summary>
  /// <returns>Random [Int64]</returns>
  public static long NextLong()
  {
    byte[] data = new byte[INT64_SIZE];
    _random.GetBytes(data);
    return BitConverter.ToInt64(data, 0);
  }

  /// <summary>
  /// Get the next random long integer to a maximum value
  /// </summary>
  /// <param name="maxValue">Maximum value</param>
  /// <returns>Random [Int64]</returns>
  public static long NextLong(long maxValue)
  {
    long result;
    do
    {
      result = NextLong();
    } while (result > maxValue || result < 0);  // Ensuring positive integers
    return result;
  }
  #endregion

  #region Random UInt64
  /// <summary>
  /// Get the next random unsigned long integer
  /// </summary>
  /// <returns>Random [UInt64]</returns>
  public static ulong NextULong()
  {
    byte[] data = new byte[INT64_SIZE];
    _random.GetBytes(data);
    return BitConverter.ToUInt64(data, 0);
  }

  /// <summary>
  /// Get the next random unsigned long to a maximum value
  /// </summary>
  /// <param name="maxValue">Maximum value</param>
  /// <returns>Random [UInt64]</returns>
  public static ulong NextULong(ulong maxValue)
  {
    ulong result;
    do
    {
      result = NextULong();
    } while (result > maxValue);
    return result;
  }
  #endregion

  #region Random Bytes
  /// <summary>
  /// Get random bytes
  /// </summary>
  /// <param name="size">Size of the random byte array</param>
  /// <returns>Random [byte array]</returns>
  public static byte[] NextBytes(long size)
  {
    byte[] data = new byte[size];
    _random.GetBytes(data);
    return data;
  }
  #endregion
}
