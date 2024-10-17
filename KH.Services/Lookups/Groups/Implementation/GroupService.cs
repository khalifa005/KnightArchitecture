using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Response;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.GroupDto.Request;
using KH.Dto.Lookups.RoleDto.Request;
using KH.Services.Lookups.Groups.Contracts;

public class GroupService : IGroupService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IServiceProvider _serviceProvider;
  private readonly IMapper _mapper;
  private readonly IHostEnvironment _env;
  private readonly ILogger<GroupService> _logger;
  public GroupService(
    IUnitOfWork unitOfWork,
    IHostEnvironment env,
    ILogger<GroupService> logger,
    IServiceProvider serviceProvider,
    IMapper mapper)
  {
    _env = env;
    _logger = logger;
    _unitOfWork = unitOfWork;
    _serviceProvider = serviceProvider;
    _mapper = mapper;
  }

  public async Task<ApiResponse<GroupResponse>> GetAsync(long id, CancellationToken cancellationToken)
  {
    var res = new ApiResponse<GroupResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<KH.Domain.Entities.lookups.Group>();

    var entityFromDB = await repository.GetAsync(id);

    if (entityFromDB == null)
      throw new Exception("Invalid Parameter");

    var entityDetailsResponse = new GroupResponse(entityFromDB);

    res.Data = entityDetailsResponse;
    return res;
  }
  public async Task<ApiResponse<List<GroupListResponse>>> GetListAsync(GroupFilterRequest request, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<KH.Domain.Entities.lookups.Group>();

    var listResult = await repository.GetAllAsync(
    include:null,
    tracking: false,
    useCache: true,
    cancellationToken: cancellationToken);

    var mappedListResult = listResult
      .Where(x => x.IsDeleted == request.IsDeleted)
      .Select(x => new GroupListResponse(x)).ToList();

    var apiResponse = new ApiResponse<List<GroupListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = mappedListResult;

    return apiResponse;
  }
  public async Task<ApiResponse<PagedResponse<GroupListResponse>>> GetPagedListAsync(GroupFilterRequest request, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<KH.Domain.Entities.lookups.Group>();

    var pagedEntities = await repository.GetPagedWithProjectionAsync<GroupListResponse>(
    pageNumber: 1,
    pageSize: 10,
    filterExpression: u => u.IsDeleted == request.IsDeleted,
    projectionExpression: u => new GroupListResponse(u),
    include: null,
    orderBy: query => query.OrderBy(u => u.Id),
    tracking: false
  );


    var entityListResponses = pagedEntities.Select(x => x).ToList();

    var pagedResponse = new PagedResponse<GroupListResponse>(
      entityListResponses,
       pagedEntities.CurrentPage,
       pagedEntities.TotalPages,
       pagedEntities.PageSize,
       pagedEntities.TotalCount);

    var apiResponse = new ApiResponse<PagedResponse<GroupListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
  public async Task<ApiResponse<string>> AddAsync(CreateGroupRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync(cancellationToken);

    try
    {
      var entity = request.ToEntity();

      var repository = _unitOfWork.Repository<KH.Domain.Entities.lookups.Group>();

      await repository.AddAsync(entity, cancellationToken: cancellationToken);
      await _unitOfWork.CommitAsync(cancellationToken);

      await _unitOfWork.CommitTransactionAsync(cancellationToken);
      repository.RemoveCache();
      res.Data = entity.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken);

      return ex.HandleException(res, _env, _logger);
    }
  }
  public async Task<ApiResponse<string>> UpdateAsync(CreateGroupRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repository = _unitOfWork.Repository<KH.Domain.Entities.lookups.Group>();
    await _unitOfWork.BeginTransactionAsync(cancellationToken);

    try
    {
      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var entityFromDb = await repository.GetAsync(request.Id.Value, tracking: true, cancellationToken: cancellationToken);

      if (entityFromDb == null)
        throw new Exception("Invalid Parameter");

      //Add the new props here ..etc
      entityFromDb.NameAr = entityAfterMapping.NameAr;
      entityFromDb.NameEn = entityAfterMapping.NameEn;
      entityFromDb.Description = entityAfterMapping.Description;

      await _unitOfWork.CommitAsync(cancellationToken);
      await _unitOfWork.CommitTransactionAsync(cancellationToken);
      repository.RemoveCache();
      res.Data = entityAfterMapping.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken);

      return ex.HandleException(res, _env, _logger);
    }
  }
  public async Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<KH.Domain.Entities.lookups.Group>();

      var entityFromDB = await repository.GetAsync(id, cancellationToken: cancellationToken);
      if (entityFromDB == null)
        throw new Exception("Invalid user");

      repository.Delete(entityFromDB);
      await _unitOfWork.CommitAsync(cancellationToken);
      repository.RemoveCache();
      res.Data = entityFromDB.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      return ex.HandleException(res, _env, _logger);
    }
  }

}

