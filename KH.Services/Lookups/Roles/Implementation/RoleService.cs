using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Request;
using KH.Services.Lookups.Roles.Contracts;

public class RoleService : IRoleService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IServiceProvider _serviceProvider;
  private readonly IMapper _mapper;
  private readonly IHostEnvironment _env;
  private readonly ILogger<RoleService> _logger;



  public RoleService(
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider,
    IMapper mapper,
    IHostEnvironment env,
    ILogger<RoleService> logger)
  {
    _unitOfWork = unitOfWork;
    _serviceProvider = serviceProvider;
    _mapper = mapper;
    _env = env;
    _logger = logger;
  }

  public async Task<ApiResponse<RoleResponse>> GetAsync(long id, CancellationToken cancellationToken)
  {
    var res = new ApiResponse<RoleResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<Role>();

    var entityFromDB = await repository.GetAsync(id,
      x =>
      x.Include(i => i.RolePermissions)
      .ThenInclude(i => i.Permission),
      cancellationToken: cancellationToken);

    if (entityFromDB == null)
      throw new Exception("Invalid Parameter");

    var entityDetailsResponse = new RoleResponse(entityFromDB);

    res.Data = entityDetailsResponse;
    return res;
  }
  public async Task<ApiResponse<List<RoleResponse>>> GetListAsync(RoleFilterRequest request, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<Role>();

    var rolesListResult = await repository.GetAllAsync(
    include: query => query.Include(r => r.SubRoles)
                           .Include(r => r.RolePermissions)
                           .ThenInclude(p => p.Permission),
    tracking: false,
    useCache: false,
    cancellationToken: cancellationToken);
    //filter after the query because we use cache 
    var mappedRolesListResult = rolesListResult
      .Where(x => x.IsDeleted == request.IsDeleted)
      .Select(x => new RoleResponse(x)).ToList();

    var apiResponse = new ApiResponse<List<RoleResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = mappedRolesListResult;

    return apiResponse;
  }
  public async Task<ApiResponse<PagedResponse<RoleListResponse>>> GetPagedListAsync(RoleFilterRequest request, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<Role>();

    var pagedEntities = await repository.GetPagedWithProjectionAsync<RoleListResponse>(
    pageNumber: request.PageIndex,
    pageSize: request.PageSize,
    filterExpression: u => (u.IsDeleted == request.IsDeleted)
    && (string.IsNullOrEmpty(request.Description) || u.NameEn.Contains(request.Description))
    && (string.IsNullOrEmpty(request.NameEn) || u.NameEn.Contains(request.NameEn))
    && (string.IsNullOrEmpty(request.NameAr) || u.NameAr.Contains(request.NameAr)),
    projectionExpression: u => new RoleListResponse(u),
    include: null,
    orderBy: query => query.OrderBy(u => u.Id),
    tracking: false,
    cancellationToken: cancellationToken
);


    var entityListResponses = pagedEntities.Select(x => x).ToList();

    var pagedResponse = new PagedResponse<RoleListResponse>(
      entityListResponses,
       pagedEntities.CurrentPage,
       pagedEntities.TotalPages,
       pagedEntities.PageSize,
       pagedEntities.TotalCount);

    var apiResponse = new ApiResponse<PagedResponse<RoleListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
  public async Task<ApiResponse<string>> AddAsync(CreateRoleRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync(cancellationToken);

    try
    {
      var entity = request.ToEntity();

      var repository = _unitOfWork.Repository<Role>();

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
  public async Task<ApiResponse<string>> UpdateAsync(CreateRoleRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repository = _unitOfWork.Repository<Role>();
    await _unitOfWork.BeginTransactionAsync(cancellationToken);

    try
    {
      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var entityFromDb = await repository.GetAsync(request.Id.Value,
        include: x => x.Include(x => x.RolePermissions),
        tracking: true,
        cancellationToken: cancellationToken);

      if (entityFromDb == null)
        throw new Exception("Invalid Parameter");

      entityFromDb.NameAr = entityAfterMapping.NameAr;
      entityFromDb.NameEn = entityAfterMapping.NameEn;
      entityFromDb.Description = entityAfterMapping.Description;

      await _unitOfWork.CommitAsync(cancellationToken);
      await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);
      repository.RemoveCache();

      res.Data = entityAfterMapping.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken);

      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }
  public async Task<ApiResponse<string>> UpdateBothRoleWithRelatedPermissionsAsync(CreateRoleRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repositoryRolePermissions = _unitOfWork.Repository<RolePermissions>();
    var repository = _unitOfWork.Repository<Role>();
    await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

    try
    {
      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var entityFromDb = await repository
        .GetAsync(request.Id.Value,
        include: x => x.Include(x => x.RolePermissions),
        tracking: true,
        cancellationToken: cancellationToken);

      if (entityFromDb == null)
        throw new Exception("Invalid Parameter");

      //Add the new props here ..etc
      entityFromDb.NameAr = entityAfterMapping.NameAr;
      entityFromDb.NameEn = entityAfterMapping.NameEn;
      entityFromDb.Description = entityAfterMapping.Description;


      if (request.HasPermissionsUpdates)
      {
        // Get matched role permissions from the database based on request's RolePermissionsIds
        var matchedRolePermissions = entityFromDb.RolePermissions
            .Where(rp => request.RolePermissionsIds.Contains(rp.PermissionId))
            .ToList();

        // Get new role permissions that don't exist in both entityFromDb and matchedRolePermissions
        var newRolePermissions = entityAfterMapping.RolePermissions
            .Where(np => !entityFromDb.RolePermissions.Any(rp => rp.PermissionId == np.PermissionId)
                      && !matchedRolePermissions.Any(mp => mp.PermissionId == np.PermissionId))
            .ToList();

        // Combine the matched and new role permissions
        var updatedRolePermissions = matchedRolePermissions.Concat(newRolePermissions).ToList();

        // Replace the RolePermissions collection, EF Core will track the changes automatically
        entityFromDb.RolePermissions = updatedRolePermissions;
      }

      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
      await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);
      repository.RemoveCache();

      res.Data = entityAfterMapping.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken: cancellationToken);

      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }
  public async Task<ApiResponse<string>> UpdateRolePermissionsAsync(UpdatedRolePermissionsRequest request, CancellationToken cancellationToken)
  {
    //may need to drestroy the cache
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repositoryRolePermissions = _unitOfWork.Repository<RolePermissions>();
    var repository = _unitOfWork.Repository<Role>();
    await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

    try
    {

      var entityFromDb = await repository.GetAsync(request.Id,
        include: x => x.Include(x => x.RolePermissions),
        tracking: true,
        cancellationToken: cancellationToken);

      if (entityFromDb == null)
        throw new Exception("Invalid Parameter");

      // Find permissions that should be removed
      var permissionsToRemove = entityFromDb.RolePermissions
          .Where(rp => !request.RolePermissionsIds.Contains(rp.PermissionId))
          .ToList();

      entityFromDb.RolePermissions = entityAfterMapping.RolePermissions;

      // Remove them from the Role's RolePermissions collection
      foreach (var permission in permissionsToRemove)
      {
        //repositoryRolePermissions.DeleteTracked(permission);


      }
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
      await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);
      repository.RemoveCache();

      res.Data = entityAfterMapping.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken: cancellationToken);

      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }
  public async Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<Role>();

      var entityFromDB = await repository.GetAsync(id, cancellationToken: cancellationToken);
      if (entityFromDB == null)
        throw new Exception("invalid-role");

      if (entityFromDB.IsDeleted == true)
        throw new Exception("already-deleted");

      repository.Delete(entityFromDB);
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
      repository.RemoveCache();

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

  public async Task<ApiResponse<string>> ReActivateAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<Role>();

      var entityFromDB = await repository.GetAsync(id, tracking: true, cancellationToken: cancellationToken);
      if (entityFromDB == null)
        throw new Exception("invalid-role");

      if (entityFromDB.IsDeleted == false)
        throw new Exception("already-activated");

      entityFromDB.IsDeleted = false;
      entityFromDB.DeletedDate = null;
      entityFromDB.DeletedById = null;
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
      repository.RemoveCache();

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


}

