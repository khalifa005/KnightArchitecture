using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Helper.Extentions.Methods
{
  public class SecureRandom
  {
    #region Constants
    private const int INT_SIZE = 2;
    private const int INT64_SIZE = 8;
    #endregion

    #region Fields
    private static RandomNumberGenerator _Random;
    #endregion

    #region Constructor
    static SecureRandom()
    {
      _Random = new RNGCryptoServiceProvider();
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
      int[] result = new int[1];

      _Random.GetBytes(data);
      Buffer.BlockCopy(data, 0, result, 0, INT_SIZE);

      return result[0];
    }

    /// <summary>
    /// Get the next random integer to a maximum value
    /// </summary>
    /// <param name="MaxValue">Maximum value</param>
    /// <returns>Random [Int32]</returns>
    public static int Next(int MaxValue)
    {
      int result = 0;

      do
      {
        result = Next();
      } while (result > MaxValue);

      return result;
    }


    /// <summary>
    /// Get the next random integer to a maximum value
    /// </summary>
    /// <param name="MaxValue">Maximum value</param>
    /// <returns>Random [Int32]</returns>
    public static int TicketIdGenerator()
    {
      int result = 0;
      int minValue = 0;

      do
      {
        result = Next();
      } while (result < minValue);

      return result;
    }

    /// <summary>
    /// Generate One Time Password
    /// </summary>
    /// <param name="otpLength"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static string GenerateOTP(int otpLength = 4, int maxValue = 9)
    {
      string otp = string.Empty;
      for (int i = 0; i < otpLength; i++)
      {
        otp += Next(maxValue);
      }
      return otp;
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
      int[] result = new int[1];

      do
      {
        _Random.GetBytes(data);
        Buffer.BlockCopy(data, 0, result, 0, INT_SIZE);
      } while (result[0] < 0);

      return (uint)result[0];
    }

    /// <summary>
    /// Get the next random unsigned integer to a maximum value
    /// </summary>
    /// <param name="MaxValue">Maximum value</param>
    /// <returns>Random [UInt32]</returns>
    public static uint NextUInt(uint MaxValue)
    {
      uint result = 0;

      do
      {
        result = NextUInt();
      } while (result > MaxValue);

      return result;
    }
    #endregion

    #region Random Int64
    /// <summary>
    /// Get the next random integer
    /// </summary>
    /// <returns>Random [Int32]</returns>
    public static long NextLong()
    {
      byte[] data = new byte[INT64_SIZE];
      long[] result = new long[1];

      _Random.GetBytes(data);
      Buffer.BlockCopy(data, 0, result, 0, INT64_SIZE);

      return result[0];
    }

    /// <summary>
    /// Get the next random unsigned long to a maximum value
    /// </summary>
    /// <param name="MaxValue">Maximum value</param>
    /// <returns>Random [UInt64]</returns>
    public static long NextLong(long MaxValue)
    {
      long result = 0;

      do
      {
        result = NextLong();
      } while (result > MaxValue);

      return result;
    }
    #endregion

    #region Random UInt32
    /// <summary>
    /// Get the next random unsigned long
    /// </summary>
    /// <returns>Random [UInt64]</returns>
    public static ulong NextULong()
    {
      byte[] data = new byte[INT64_SIZE];
      long[] result = new long[1];

      do
      {
        _Random.GetBytes(data);
        Buffer.BlockCopy(data, 0, result, 0, INT64_SIZE);
      } while (result[0] < 0);

      return (ulong)result[0];
    }

    /// <summary>
    /// Get the next random unsigned long to a maximum value
    /// </summary>
    /// <param name="MaxValue">Maximum value</param>
    /// <returns>Random [UInt64]</returns>
    public static ulong NextULong(ulong MaxValue)
    {
      ulong result = 0;

      do
      {
        result = NextULong();
      } while (result > MaxValue);

      return result;
    }
    #endregion

    #region Random Bytes
    /// <summary>
    /// Get random bytes
    /// </summary>
    /// <param name="data">Random [byte array]</param>
    public static byte[] NextBytes(long Size)
    {
      byte[] data = new byte[Size];
      _Random.GetBytes(data);
      return data;
    }
    #endregion
  }
}
