using FluentEmail.Core;
using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;
using Microsoft.Extensions.Options;
using KH.Services.Emails.Contracts;

public class EmailTrackerQueryService : IEmailTrackerQueryService
{
  private readonly IUserQueryService _userQueryService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly MailSettings _mailSettings;
  private readonly MailTemplatesSettings _mailTemplatesSettings;
  private IFluentEmailFactory _fluentEmail;
  private readonly ILogger<EmailService> _logger;
    private readonly IHostEnvironment _env;
  public EmailTrackerQueryService(
    IFluentEmailFactory fluentEmail,
    IUserQueryService userQueryService,
    IUnitOfWork unitOfWork,
    IOptions<MailSettings> mailSettings,
    IOptions<MailTemplatesSettings> mailTemplatesSettings,
    IHostEnvironment env,
    ILogger<EmailService> loggerFactory)
  {
    _fluentEmail = fluentEmail;
    _mailSettings = mailSettings.Value;
    _mailTemplatesSettings = mailTemplatesSettings.Value;
    _logger = loggerFactory;
    _userQueryService = userQueryService;
    _unitOfWork = unitOfWork;
    _env = env;
  }

  
  public async Task<ApiResponse<EmailTrackerResponse>> GetAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<EmailTrackerResponse>? res = new ApiResponse<EmailTrackerResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<EmailTracker>();

    //light user query to make sure the user exist
    var entityFromDB = await repository.GetAsync(id,cancellationToken:cancellationToken);

    if (entityFromDB == null)
    {
      res.StatusCode = (int)StatusCodes.Status400BadRequest;
      res.ErrorMessage = "invalid";
    }

    EmailTrackerResponse entityResponse = new EmailTrackerResponse(entityFromDB);

    res.Data = entityResponse;
    return res;
  }
  public async Task<ApiResponse<PagedResponse<EmailTrackerResponse>>> GetListAsync(MailRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<PagedResponse<EmailTrackerResponse>> apiResponse = new ApiResponse<PagedResponse<EmailTrackerResponse>>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<EmailTracker>();

    var pagedEntities = await repository.GetPagedWithProjectionAsync<EmailTrackerResponse>(
    pageNumber: request.PageIndex,
    pageSize: request.PageSize,
    filterExpression: u =>
    u.IsSent == request.IsSent
    && u.ModelId == request.ModelId
    && u.Model == request.Model, // Filter by

    projectionExpression: u => new EmailTrackerResponse(u),
    orderBy: query => query.OrderBy(u => u.Id),  // Sort by Id
    tracking: false,  // Disable tracking for read-only queries
    cancellationToken: cancellationToken
);

    var entitiesResponses = pagedEntities.Select(x => x).ToList();

    var pagedResponse = new PagedResponse<EmailTrackerResponse>(
      entitiesResponses,
       pagedEntities.CurrentPage,
       pagedEntities.TotalPages,
       pagedEntities.PageSize,
       pagedEntities.TotalCount);

    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
}
