namespace KH.BuildingBlocks.Responses
{
  public class ApiException : ApiResponse<object>
  {
    public ApiException(int statusCode, string errorMessage = null, string errorMessageAr = null,
                        string errorDetails = null, string errorDetailsAr = null)
        : base(statusCode, errorMessage, errorMessageAr: errorMessageAr)
    {
      ErrorDetails = errorDetails;
      ErrorDetailsAr = errorDetailsAr;
    }

    public string ErrorDetails { get; set; }
    public string ErrorDetailsAr { get; set; }
  }
}
