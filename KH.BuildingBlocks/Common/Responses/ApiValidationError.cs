namespace KH.BuildingBlocks.Apis.Responses;

public class ApiValidationError : ApiResponse<object>
{
  public ApiValidationError() : base((int)HttpStatusCode.BadRequest) { }

  public Dictionary<string, IEnumerable<string>> ErrorDetails { get; set; } = new Dictionary<string, IEnumerable<string>>();
  public Dictionary<string, IEnumerable<string>> ErrorDetailsAr { get; set; } = new Dictionary<string, IEnumerable<string>>();

  public ApiResponse<object> HandleApiResponseError()
  {
    var apiResponse = new ApiResponse<object>((int)HttpStatusCode.BadRequest);

    StringBuilder errorDetailsAr = new StringBuilder();

    foreach (var error in ErrorDetailsAr)
    {
      if (error.Value.Count() > 0)
      {
        errorDetailsAr.Append(error.Key);
        errorDetailsAr.Append(": ");
        errorDetailsAr.Append(error.Value.FirstOrDefault());
        errorDetailsAr.Append(',');
      }

    }

    StringBuilder errorDetails = new StringBuilder();

    foreach (var error in ErrorDetails)
    {
      if (error.Value.Count() > 0)
      {
        errorDetails.Append(error.Key);
        errorDetails.Append(": ");
        errorDetails.Append(error.Value.FirstOrDefault());
        errorDetails.Append(',');
      }

    }

    apiResponse.ErrorMessageAr = !string.IsNullOrEmpty(errorDetailsAr.ToString()) ? errorDetailsAr.ToString() : ErrorMessageAr;
    apiResponse.ErrorMessage = !string.IsNullOrEmpty(errorDetails.ToString()) ? errorDetails.ToString() : ErrorMessage;
    if (!string.IsNullOrEmpty(errorDetails.ToString()))
    {
      try
      {
        apiResponse.Errors = errorDetails.ToString().Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
      }
      catch (Exception)
      {
        //log the problem 
      }
    }


    return apiResponse;
  }

}
