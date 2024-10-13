using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KH.BuildingBlocks.Apis.Responses;

public struct HeroResult<T>
{
  public HeroResult(bool ist, T val)
  {
    IsTrue = ist;
    Value = val;
    //_exceptionObject = null;
    Message = null;
  }

  public HeroResult(bool ist, T val, string msg)
      : this(ist, val) => Message = msg;

  /// <summary>
  /// Hold the value of the return value
  /// </summary>
  public T Value { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether this instance is true.
  /// </summary>
  /// <value><c>true</c> if this instance is true; otherwise, <c>false</c>.</value>
  public bool IsTrue { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether this instance is false.
  /// </summary>
  /// <value><c>true</c> if this instance is false; otherwise, <c>false</c>.</value>
  public bool IsFalse => !IsTrue;

  public string? Message { get; set; }

  public void SetTrue(T val)
  {
    IsTrue = true;
    Value = val;
  }

  public void SetFalse() => IsTrue = false;

  /// <summary>
  /// Sets the false value by passing exception
  /// </summary>
  /// <param name="e">The e.</param>
  public void SetFalse(Exception e)
  {
    IsTrue = false; ;
    //ExceptionObject = e;
    Message = FormatResponseError(e.Message) /*+ " - source : " + e.Source + " - stack trace : " + e.StackTrace*/;
  }

  /// <summary>
  /// Return false return value.
  /// </summary>
  /// <returns></returns>
  public static HeroResult<T> False()
  {
    var r = new HeroResult<T>();
    r.SetFalse();

    return r;
  }

  /// <summary>
  /// Return false return value.
  /// </summary>
  /// <param name="e">The e.</param>
  /// <returns></returns>
  public static HeroResult<T> False(Exception e)
  {
    var r = new HeroResult<T>();
    r.SetFalse(e);

    return r;
  }

  public static HeroResult<T> False(string errorMsg)
  {
    var r = new HeroResult<T>();
    r.SetFalse();
    r.Message = FormatResponseError(errorMsg);

    return r;
  }

  public static HeroResult<T> True(T value)
  {
    var r = new HeroResult<T>();
    r.SetTrue(value);

    return r;
  }

  public static implicit operator T(HeroResult<T> returnValue) => returnValue.Value;

  public static string FormatResponseError(string errorMessage)
  {
    try
    {
      var errorObj = JsonConvert.DeserializeObject(errorMessage);
      if (errorObj == null)
        return errorMessage;

      JObject jObject = JObject.FromObject(errorObj);
      return jObject["errors"]?.LastOrDefault()?.Last?.ToString() ?? errorMessage;
    }
    catch (Exception)
    {
      return errorMessage;
    }
  }
}

public class HttpModelDTO
{

  //[ModelBinder(BinderType = typeof(FormDataJsonBinder))]
  public dynamic Data { get; set; }

  public List<GenericFilesDTO> NewFiles { get; set; }

}

public class GenericFilesDTO
{
  public string NameOfFile { get; set; }
  public IFormFile File { get; set; }
}
