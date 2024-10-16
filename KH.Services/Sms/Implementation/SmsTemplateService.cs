using KH.BuildingBlocks.Apis.Services;
using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;
using KH.Services.Sms.Contracts;
using Microsoft.Extensions.Options;

public class SmsTemplateService : ISmsTemplateService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly SmsSettings _smsProviderSettings;
  private readonly CustomHttpRequestService _httpRequestHelper;
  private readonly IHostEnvironment _env;
  private readonly ILogger<SmsTemplateService> _logger;
  public SmsTemplateService(
    IUnitOfWork unitOfWork,
    IOptions<SmsSettings> smsProviderSettings,
    CustomHttpRequestService httpRequestHelper,
    IHostEnvironment env,
    ILogger<SmsTemplateService> loggerFactory)
  {
    _smsProviderSettings = smsProviderSettings.Value;
    _logger = loggerFactory;
    _env = env;
    _httpRequestHelper = httpRequestHelper;
    _unitOfWork = unitOfWork;
  }

  public async Task<ApiResponse<SmsTemplateResponse>> GetSmsTemplateAsync(string smsType, CancellationToken cancellationToken)
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
  public async Task<ApiResponse<PagedResponse<SmsTemplateResponse>>> GetSmsTemplateListAsync(SmsTrackerFilterRequest request, CancellationToken cancellationToken)
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
  public async Task<ApiResponse<string>> AddSmsTemplateAsync(CreateSmsTemplateRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

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

      await repository.AddAsync(request.ToEntity(), cancellationToken: cancellationToken);

      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

      await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);

      res.Data = request.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken: cancellationToken);

      return ex.HandleException(res, _env, _logger);
    }
  }

  public async Task<ApiResponse<string>> UpdateAsync(CreateSmsTemplateRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repository = _unitOfWork.Repository<SmsTemplate>();
    if (!request.Id.HasValue)
      throw new Exception("id is required");

    var entityFromDb = await repository
      .GetAsync(request.Id.Value,
      tracking: true,
      cancellationToken: cancellationToken);

    if (entityFromDb == null)
      throw new Exception("Invalid Parameter");

    entityFromDb.TextAr = entityAfterMapping.TextAr;
    entityFromDb.TextEn = entityAfterMapping.TextEn;
    entityFromDb.SmsType = entityAfterMapping.SmsType;

    await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

    res.Data = entityAfterMapping.Id.ToString();
    return res;
  }
  public async Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<SmsTemplate>();

      var entityFromDB = await repository.GetAsync(id, cancellationToken: cancellationToken);
      if (entityFromDB == null)
        throw new Exception("Invalid user");

      repository.DeleteTracked(entityFromDB);
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

      res.Data = entityFromDB.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {

      return ex.HandleException(res, _env, _logger);
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
