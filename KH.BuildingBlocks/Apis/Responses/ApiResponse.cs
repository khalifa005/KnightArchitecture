namespace KH.BuildingBlocks.Apis.Responses;

public class ApiResponse<T> where T : class
{
  public ApiResponse(int statusCode, string errorMessage = null, string errorMessageAr = null)
  {
    StatusCode = statusCode;
    ErrorMessage = errorMessage;
    ErrorMessageAr = errorMessageAr;
    Errors = new List<string>();
    assignErrorDetails();
  }

  public T Data { get; set; } = null;
  public int StatusCode { get; set; }

  public ICollection<string> Errors { get; set; }
  public string ErrorMessage { get; set; }
  public string ErrorCode { get; set; }
  public string ErrorMessageAr { get; set; }

  void assignErrorDetails()
  {
    switch (StatusCode)
    {
      case (int)HttpStatusCode.BadRequest:
        ErrorMessage = ErrorMessage ?? "A bad request, you have made";
        ErrorMessageAr = ErrorMessageAr ?? "البيانات التي تم ارسالها غير صحيحة";
        ErrorCode = "ERO_001";
        break;
      case (int)HttpStatusCode.Unauthorized:
        ErrorMessage = ErrorMessage ?? "Authorized, you are not";
        ErrorMessageAr = ErrorMessageAr ?? "انت غير مصرح لك بالدخول";
        ErrorCode = "ERO_002";
        break;
      case (int)HttpStatusCode.NotFound:
        ErrorMessage = ErrorMessage ?? "Resource found, it was not";
        ErrorMessageAr = ErrorMessageAr ?? "اللينك المدخل غير موجود لدينا";
        ErrorCode = "ERO_003";
        break;
      case (int)HttpStatusCode.InternalServerError:
        ErrorMessage = ErrorMessage ?? "Errors found in application, please contact with the acig support team";
        ErrorMessageAr = ErrorMessageAr ?? "يوجد خطا في النظام من فضلك تواصل مع فريق دعم ";
        ErrorCode = "ERO_005";
        break;
      case (int)HttpStatusCode.Forbidden:
        ErrorMessage = ErrorMessage ?? "you are not Authorized to view this page";
        ErrorMessageAr = ErrorMessageAr ?? "انت غير مصرح لك بالدخول لهذه الصفحة";
        ErrorCode = "ERO_006";
        break;
      case (int)HttpStatusCode.OK:
        ErrorMessage = "";
        ErrorMessageAr = "";
        ErrorCode = "";
        break;
    }
  }



}
