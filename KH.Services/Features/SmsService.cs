using KH.BuildingBlocks.Apis;
using KH.BuildingBlocks.Apis.Constant;
using KH.BuildingBlocks.Apis.Enums;
using KH.BuildingBlocks.Apis.Responses;
using KH.BuildingBlocks.Settings;
using KH.Dto.Models.SMSDto.Form;
using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

public class SmsService : ISmsService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly SmsSettings _smsProviderSettings;
  private readonly HttpRequestHelper _httpRequestHelper;

  private readonly ILogger<SmsService> _loggerFactory;
  public SmsService(
    IUnitOfWork unitOfWork,
    IOptions<SmsSettings> smsProviderSettings,
    HttpRequestHelper httpRequestHelper,
    ILogger<SmsService> loggerFactory)
  {
    _smsProviderSettings = smsProviderSettings.Value;
    _loggerFactory = loggerFactory;
    _httpRequestHelper = httpRequestHelper;
    _unitOfWork = unitOfWork;
  }

  public string BuildFormattedSmsApiUrl(string smsApiUrl, string userName, string password, string phoneNumber, string message)
  {
    const string SAUDI_COUNTRY_CODE = ApplicationConstant.MOBILE_NUMBER_PREFIX;
    const string EGYPT_COUNTRY_CODE = "20";
    const string USA_COUNTRY_CODE = "1";

    const string SAUDI_MOBILE_PREFIX = "05";
    const string EGYPT_MOBILE_PREFIX = "01";
    const string USA_MOBILE_PREFIX = "1";

    if (!string.IsNullOrEmpty(phoneNumber))
    {
      phoneNumber = phoneNumber.Trim();

      // Handle Saudi Arabia phone numbers
      if (phoneNumber.StartsWith(SAUDI_MOBILE_PREFIX))
      {
        // Remove the leading '0' and add the Saudi country code
        phoneNumber = SAUDI_COUNTRY_CODE + phoneNumber.Substring(1);
      }
      else if (phoneNumber.StartsWith(SAUDI_COUNTRY_CODE))
      {
        // No changes needed for already formatted Saudi numbers
      }

      // Handle Egypt phone numbers
      else if (phoneNumber.StartsWith(EGYPT_MOBILE_PREFIX))
      {
        // Remove the leading '0' and add the Egypt country code
        phoneNumber = EGYPT_COUNTRY_CODE + phoneNumber.Substring(1);
      }
      else if (phoneNumber.StartsWith(EGYPT_COUNTRY_CODE))
      {
        // No changes needed for already formatted Egypt numbers
      }

      // Handle USA phone numbers
      else if (phoneNumber.StartsWith(USA_MOBILE_PREFIX) && phoneNumber.Length == 10)
      {
        // Add the USA country code to 10-digit numbers
        phoneNumber = USA_COUNTRY_CODE + phoneNumber;
      }
      else if (phoneNumber.StartsWith(USA_COUNTRY_CODE))
      {
        // No changes needed for already formatted USA numbers
      }

      // Handle case where the number starts with something else (assumed as a local number)
      else
      {
        // Add a default country code if needed (Saudi Arabia by default in this example)
        phoneNumber = SAUDI_COUNTRY_CODE + phoneNumber;
      }
    }

    string formattedMessage = HttpUtility.UrlEncode(message).Trim();
    string url = $"{smsApiUrl}UserName={userName}&Password={password}&MessageType=text&Recipients={phoneNumber}&SenderName=ACIG&MessageText={formattedMessage}";

    //example output
    //https://ht.deewan.sa:8443/Send.aspx?UserName=external&Password=pass&MessageType=text&Recipients=966566285570&SenderName=Khalifa&MessageText=external

    return url;
  }
  private async Task<ApiResponse<SmsTracker>> ApplySendSmsAsync(string url, SmsTracker smsTrackerRequest)
  {
    var response = new ApiResponse<SmsTracker>((int)HttpStatusCode.OK);

    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

    // If SMS provider is active, attempt to send the SMS
    if (_smsProviderSettings.IsActive)
    {
      var smsSendResult = await _httpRequestHelper.GetRequestAsync<string>(url, "");

      if (smsSendResult.IsTrue)
      {
        smsTrackerRequest.IsSent = true;
        smsTrackerRequest.Status = SmsStatusEnum.Success.ToString();
      }
      else
      {
        smsTrackerRequest.IsSent = false;
        smsTrackerRequest.Status = SmsStatusEnum.Fail.ToString();
        smsTrackerRequest.FailureReasons = smsSendResult.Message;
        response.StatusCode = StatusCodes.Status400BadRequest;
      }
    }
    else
    {
      // If the SMS provider is not active, generate a text file with SMS content
      smsTrackerRequest.IsSent = true;
      smsTrackerRequest.Status = SmsStatusEnum.Success.ToString();

      string smsContent = $"To: {smsTrackerRequest.MobileNumber}\nMessage: {smsTrackerRequest.Message}\nStatus: {smsTrackerRequest.Status}";
      string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Fake SMS");

      if (!Directory.Exists(directoryPath))
      {
        Directory.CreateDirectory(directoryPath);
      }

      string fileName = $"sms_{smsTrackerRequest.MobileNumber}_{DateTime.Now:yyyyMMddHHmmss}.txt";
      string filePath = Path.Combine(directoryPath, fileName);

      await File.WriteAllTextAsync(filePath, smsContent);
    }

    response.Data = smsTrackerRequest;
    return response;
  }
  public async Task<ApiResponse<SmsTrackerResponse>> GetSmsTrackerAsync(long id)
  {
    var res = new ApiResponse<SmsTrackerResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<SmsTracker>();

    var entityFromDB = await repository.GetAsync(id);

    if (entityFromDB == null)
    {
      res.StatusCode = (int)StatusCodes.Status400BadRequest;
      res.ErrorMessage = "invalid";
      return res;
    }

    res.Data = new SmsTrackerResponse(entityFromDB);
    return res;
  }
  public async Task<ApiResponse<PagedResponse<SmsTrackerResponse>>> GetSmsTrackerListAsync(SmsTrackerFilterRequest request)
  {
    var apiResponse = new ApiResponse<PagedResponse<SmsTrackerResponse>>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<SmsTracker>();

    var pagedEntities = await repository.GetPagedWithProjectionAsync<SmsTrackerResponse>(
    pageNumber: request.PageIndex,
    pageSize: request.PageSize,
    filterExpression: u =>
    u.IsSent == request.IsSent
    && u.ModelId == request.ModelId
    && u.Model == request.Model, // Filter by

    projectionExpression: u => new SmsTrackerResponse()
    {
      Id = u.Id,
      Model = u.Model,
      MobileNumber = u.MobileNumber,
      ModelId = u.ModelId,
      FailureReasons = u.FailureReasons,
      IsSent = u.IsSent,
      Message = u.Message,
      Status = u.Status,

    },
    orderBy: query => query.OrderBy(u => u.Id),  // Sort by Id
    tracking: false  // Disable tracking for read-only queries
);

    var entitiesResponses = pagedEntities.Select(x => x).ToList();

    var pagedResponse = new PagedResponse<SmsTrackerResponse>(
      entitiesResponses,
       pagedEntities.CurrentPage,
       pagedEntities.TotalPages,
       pagedEntities.PageSize,
       pagedEntities.TotalCount);

    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
  public async Task<ApiResponse<string>> SendSmsAsync(SmsTrackerForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    bool isModelExists = Enum.IsDefined(typeof(ModelEnum), request.Model);

    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      if (request.ModelId == null)
        throw new Exception("Invalid Parameter");

      if (request.Model == null)
        throw new Exception("Invalid Parameter");

      if (request.Message == null)
        throw new Exception("Invalid Parameter");

      if (request.MobileNumber == null)
        throw new Exception("Invalid Parameter");

      var smsEntity = request.ToEntity();

      // If the SMS status is not "Scheduled", mark it as not sent and return early
      if (smsEntity.Status == SmsStatusEnum.Scheduled.ToString())
      {
        smsEntity.IsSent = false;
      }
      else
      {
        string url = this.BuildFormattedSmsApiUrl(
       _smsProviderSettings.SmsApiUrl,
       _smsProviderSettings.UserName,
       _smsProviderSettings.Password,
       request.MobileNumber,
       request.Message);

        var result = await ApplySendSmsAsync(url, smsEntity);
        smsEntity = result.Data;
      }

      var repository = _unitOfWork.Repository<SmsTracker>();

      await repository.AddAsync(smsEntity);

      await _unitOfWork.CommitAsync();


      res.Data = smsEntity.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }
  public async Task<ApiResponse<string>> ResendAsync(SmsTracker request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    bool isModelExists = Enum.IsDefined(typeof(ModelEnum), request.Model);

    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      if (request.ModelId == null)
        throw new Exception("Invalid Parameter");

      if (request.Model == null)
        throw new Exception("Invalid Parameter");

      if (request.Message == null)
        throw new Exception("Invalid Parameter");

      if (request.MobileNumber == null)
        throw new Exception("Invalid Parameter");

      var smsEntity = request;

      string url = this.BuildFormattedSmsApiUrl(
            _smsProviderSettings.SmsApiUrl,
            _smsProviderSettings.UserName,
            _smsProviderSettings.Password,
            smsEntity.MobileNumber,
            smsEntity.Message);

      var result = await ApplySendSmsAsync(url, smsEntity);
      smsEntity = result.Data;

      var repository = _unitOfWork.Repository<SmsTracker>();

      //await repository.AddAsync(smsEntity);

      await _unitOfWork.CommitAsync();


      res.Data = smsEntity.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }
}
