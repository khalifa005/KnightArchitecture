using KH.Domain.Entities.lookups;
using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;
using KH.Dto.Lookups.RoleDto.Request;
using KH.Services.Lookups.Departments.Contracts;

public class DepartmentService : IDepartmentService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IServiceProvider _serviceProvider;
  private readonly IMapper _mapper;
  private readonly IHostEnvironment _env;
  private readonly ILogger<DepartmentService> _logger;
  public DepartmentService(
    IUnitOfWork unitOfWork,
    IHostEnvironment env,
    ILogger<DepartmentService> logger,
    IServiceProvider serviceProvider,
    IMapper mapper)
  {
    _env = env;
    _logger = logger;
    _unitOfWork = unitOfWork;
    _serviceProvider = serviceProvider;
    _mapper = mapper;

  }

  public async Task<ApiResponse<DepartmentResponse>> GetAsync(long id, CancellationToken cancellationToken)
  {
    var res = new ApiResponse<DepartmentResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<Department>();

    var entityFromDB = await repository.GetAsync(id);

    if (entityFromDB == null)
      throw new Exception("Invalid Parameter");

    var entityDetailsResponse = new DepartmentResponse(entityFromDB);

    res.Data = entityDetailsResponse;
    return res;
  }

  public async Task<ApiResponse<List<DepartmentListResponse>>> GetListAsync(DepartmentFilterRequest request, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<Department>();

    var listResult = await repository.GetAllAsync(
    include:null,
    tracking: false,
    useCache: true,
    cancellationToken: cancellationToken);

    var mappedlistResult = listResult
      .Where(x => x.IsDeleted == request.IsDeleted)
      .Select(x => new DepartmentListResponse(x)).ToList();

    var apiResponse = new ApiResponse<List<DepartmentListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = mappedlistResult;

    return apiResponse;
  }
  public async Task<ApiResponse<PagedList<DepartmentListResponse>>> GetPagedListAsync(DepartmentFilterRequest request, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<Department>();

    var pagedEntities = await repository.GetPagedWithProjectionAsync<DepartmentListResponse>(
    pageNumber: 1,
    pageSize: 10,
    filterExpression: u => u.IsDeleted == request.IsDeleted,
    projectionExpression: u => new DepartmentListResponse(u),
    include: null,
    orderBy: query => query.OrderBy(u => u.Id),
    tracking: false
);

    var apiResponse = new ApiResponse<PagedList<DepartmentListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = pagedEntities;

    return apiResponse;
  }
  public async Task<ApiResponse<string>> AddAsync(CreateDepartmentRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync(cancellationToken);

    try
    {
      var entity = request.ToEntity();

      var repository = _unitOfWork.Repository<Department>();

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
  public async Task<ApiResponse<string>> UpdateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repository = _unitOfWork.Repository<Department>();
    await _unitOfWork.BeginTransactionAsync(cancellationToken);

    try
    {
      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var entityFromDb = await repository.GetAsync(request.Id.Value, tracking: true, cancellationToken: cancellationToken);

      if (entityFromDb == null)
        throw new Exception("Invalid Parameter");

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
      var repository = _unitOfWork.Repository<Department>();

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


