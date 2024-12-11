using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.PermissionsDto.Request;
using KH.Dto.Lookups.PermissionsDto.Response;
using KH.Dto.Lookups.RoleDto.Request;
using KH.Services.Lookups.Permissions.Contracts;

public class PermissionService : IPermissionService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IServiceProvider _serviceProvider;
  private readonly IMapper _mapper;
  private readonly IHostEnvironment _env;
  private readonly ILogger<PermissionService> _logger;

  public PermissionService(
    IUnitOfWork unitOfWork,
    ILogger<PermissionService> logger,
    IHostEnvironment env,
    IServiceProvider serviceProvider,
    IMapper mapper)
  {
    _unitOfWork = unitOfWork;
    _logger = logger;
    _env = env;
    _serviceProvider = serviceProvider;
    _mapper = mapper;
  }

  public async Task<ApiResponse<PermissionResponse>> GetAsync(long id, CancellationToken cancellationToken)
  {
    var res = new ApiResponse<PermissionResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<Permission>();

    var entityFromDB = await repository.GetAsync(id);

    if (entityFromDB == null)
      throw new Exception("Invalid Parameter");

    var entityDetailsResponse = new PermissionResponse(entityFromDB);

    res.Data = entityDetailsResponse;
    return res;
  }
  public async Task<ApiResponse<string>> AddAsync(CreatePermissionRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync(cancellationToken);

    try
    {

      var entity = request.ToEntity();

      var repository = _unitOfWork.Repository<Permission>();

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
  public async Task<ApiResponse<string>> UpdateAsync(CreatePermissionRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repository = _unitOfWork.Repository<Permission>();
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
      var repository = _unitOfWork.Repository<Permission>();

      var entityFromDB = await repository.GetAsync(id, cancellationToken: cancellationToken);
      if (entityFromDB == null)
        throw new Exception("Invalid");

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

  public async Task<ApiResponse<List<PermissionResponse>>> GetListAsync(CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<Permission>();

    // Apply the filter directly in the repository call
    var listResult = await repository.GetAllAsync(
        filter: x => !x.IsDeleted, // Filter directly in the repository
        include: null,
        useCache: true,
        cancellationToken: cancellationToken);

    // Map the results
    var mappedListResult = listResult
        .Select(x => new PermissionResponse(x))
        .ToList();

    // Prepare API response
    var apiResponse = new ApiResponse<List<PermissionResponse>>((int)HttpStatusCode.OK)
    {
      Data = mappedListResult
    };

    return apiResponse;
  }


  public async Task<ApiResponse<PagedList<PermissionResponse>>> GetListAsync(
    PermissionFilterRequest filterRequest,
    CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<Permission>();

    // Build the filter expression
    Expression<Func<Permission, bool>> filter = x =>
        (!filterRequest.IsDeleted.HasValue || x.IsDeleted == filterRequest.IsDeleted.Value) &&
        (!filterRequest.RoleIds.Any() || x.RolePermissions.Any(rp => filterRequest.RoleIds.Contains(rp.RoleId)));

    // Define the projection expression
    Expression<Func<Permission, PermissionResponse>> projection = x => new PermissionResponse(x);

    // Fetch the paged data with projection
    var pagedResult = await repository.GetPagedWithProjectionAsync(
        pageNumber: filterRequest.PageIndex,
        pageSize: filterRequest.PageSize,
        filterExpression: filter,
        projectionExpression: projection,
        include: null,
        orderBy: null,
        tracking: false,
        cancellationToken: cancellationToken);

    // Prepare the API response
    var apiResponse = new ApiResponse<PagedList<PermissionResponse>>((int)HttpStatusCode.OK)
    {
      Data = pagedResult
    };

    return apiResponse;
  }


}

