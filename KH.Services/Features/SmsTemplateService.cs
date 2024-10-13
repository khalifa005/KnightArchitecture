using KH.BuildingBlocks.Apis.Responses;
using KH.BuildingBlocks.Apis.Services;
using KH.BuildingBlocks.Localizatoin.Enum;
using KH.BuildingBlocks.Settings;
using KH.Dto.Models.SMSDto.Form;
using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class SmsTemplateService : ISmsTemplateService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly SmsSettings _smsProviderSettings;
  private readonly CustomHttpRequestService _httpRequestHelper;

  private readonly ILogger<SmsTemplateService> _loggerFactory;
  public SmsTemplateService(
    IUnitOfWork unitOfWork,
    IOptions<SmsSettings> smsProviderSettings,
    CustomHttpRequestService httpRequestHelper,
    ILogger<SmsTemplateService> loggerFactory)
  {
    _smsProviderSettings = smsProviderSettings.Value;
    _loggerFactory = loggerFactory;
    _httpRequestHelper = httpRequestHelper;
    _unitOfWork = unitOfWork;
  }

  public async Task<ApiResponse<SmsTemplateResponse>> GetSmsTemplateAsync(string smsType)
  {
    var res = new ApiResponse<SmsTemplateResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<SmsTemplate>();

    var entityFromDB = await repository.GetByExpressionAsync(u => u.SmsType == smsType);


    if (entityFromDB == null)
    {
      res.StatusCode = (int)StatusCodes.Status400BadRequest;
      res.ErrorMessage = "invalid";
      return res;
    }

    res.Data = new SmsTemplateResponse(entityFromDB);
    return res;
  }
  public async Task<ApiResponse<PagedResponse<SmsTemplateResponse>>> GetSmsTemplateListAsync(SmsTrackerFilterRequest request)
  {
    var apiResponse = new ApiResponse<PagedResponse<SmsTemplateResponse>>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<SmsTemplate>();

    var pagedEntities = await repository.GetPagedWithProjectionAsync<SmsTemplateResponse>(
    pageNumber: request.PageIndex,
    pageSize: request.PageSize,
    filterExpression: u =>
    u.IsDeleted == request.IsDeleted,

    projectionExpression: u => new SmsTemplateResponse(u),
    orderBy: query => query.OrderBy(u => u.Id),  // Sort by Id
    tracking: false  // Disable tracking for read-only queries
);

    var entitiesResponses = pagedEntities.Select(x => x).ToList();

    var pagedResponse = new PagedResponse<SmsTemplateResponse>(
      entitiesResponses,
       pagedEntities.CurrentPage,
       pagedEntities.TotalPages,
       pagedEntities.PageSize,
       pagedEntities.TotalCount);

    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
  public async Task<ApiResponse<string>> AddSmsTemplateAsync(SmsTemplateForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync();

    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      if (request.SmsType == null)
        throw new Exception("Invalid Parameter");

      if (request.TextEn == null)
        throw new Exception("Invalid Parameter");

      if (request.TextAr == null)
        throw new Exception("Invalid Parameter");

      var repository = _unitOfWork.Repository<SmsTemplate>();

      await repository.AddAsync(request.ToEntity());

      await _unitOfWork.CommitAsync();

      await _unitOfWork.CommitTransactionAsync();

      res.Data = request.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync();

      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }

  public async Task<ApiResponse<string>> UpdateAsync(SmsTemplateForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repository = _unitOfWork.Repository<SmsTemplate>();
    if (!request.Id.HasValue)
      throw new Exception("id is required");

    var entityFromDb = await repository.GetAsync(request.Id.Value, tracking: true);

    if (entityFromDb == null)
      throw new Exception("Invalid Parameter");

    entityFromDb.TextAr = entityAfterMapping.TextAr;
    entityFromDb.TextEn = entityAfterMapping.TextEn;
    entityFromDb.SmsType = entityAfterMapping.SmsType;

    await _unitOfWork.CommitAsync();

    res.Data = entityAfterMapping.Id.ToString();
    return res;
  }
  public async Task<ApiResponse<string>> DeleteAsync(long id)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<SmsTemplate>();

      var entityFromDB = await repository.GetAsync(id);
      if (entityFromDB == null)
        throw new Exception("Invalid user");

      repository.DeleteTracked(entityFromDB);
      await _unitOfWork.CommitAsync();

      res.Data = entityFromDB.Id.ToString();
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

  public string ReplaceWelcomeSmsPlaceholders(string template, User user)
  {
    return template
        .Replace("{FirstName}", user.FirstName ?? string.Empty)
        .Replace("{LastName}", user.LastName ?? string.Empty)
        .Replace("{OtpCode}", user.OtpCode ?? string.Empty)
        .Replace("{Username}", user.Username ?? string.Empty);
  }

  public string GetTemplateForLanguage(SmsTemplateResponse smsTemplate, LanguageEnum language)
  {
    return language == LanguageEnum.English ? smsTemplate.TextEn : smsTemplate.TextAr;
  }

}
