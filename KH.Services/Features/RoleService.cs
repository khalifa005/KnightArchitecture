using KH.Dto.lookups.RoleDto.Form;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Request;

public class RoleService : IRoleService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IServiceProvider _serviceProvider;
  private readonly IMapper _mapper;

  public RoleService(
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider,
    IMapper mapper)
  {
    _unitOfWork = unitOfWork;
    _serviceProvider = serviceProvider;
    _mapper = mapper;
  }

  public async Task<ApiResponse<RoleResponse>> GetAsync(long id)
  {
    var res = new ApiResponse<RoleResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<Role>();

    var entityFromDB = await repository.GetAsync(id,
      x =>
      x.Include(i => i.RolePermissions)
      .ThenInclude(i => i.Permission));

    if (entityFromDB == null)
      throw new Exception("Invalid Parameter");

    var entityDetailsResponse = new RoleResponse(entityFromDB);

    res.Data = entityDetailsResponse;
    return res;
  }
  public async Task<ApiResponse<PagedResponse<RoleListResponse>>> GetListAsync(RoleFilterRequest request)
  {
    var repository = _unitOfWork.Repository<Role>();

    var pagedEntities = await repository.GetPagedWithProjectionAsync<RoleListResponse>(
    pageNumber: 1,
    pageSize: 10,
    filterExpression: u => u.IsDeleted == request.IsDeleted,
    projectionExpression: u => new RoleListResponse(u),
    include: null,
    orderBy: query => query.OrderBy(u => u.Id),
    tracking: false
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
  public async Task<ApiResponse<string>> AddAsync(RoleForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync();

    try
    {
      var entity = request.ToEntity();

      var repository = _unitOfWork.Repository<Role>();

      await repository.AddAsync(entity);
      await _unitOfWork.CommitAsync();

      await _unitOfWork.CommitTransactionAsync();

      res.Data = entity.Id.ToString();
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
  public async Task<ApiResponse<string>> UpdateAsync(RoleForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repository = _unitOfWork.Repository<Role>();
    await _unitOfWork.BeginTransactionAsync();

    try
    {
      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var entityFromDb = await repository.GetAsync(request.Id.Value, include: x => x.Include(x => x.RolePermissions), tracking: true);

      if (entityFromDb == null)
        throw new Exception("Invalid Parameter");

      entityFromDb.NameAr = entityAfterMapping.NameAr;
      entityFromDb.NameEn = entityAfterMapping.NameEn;
      entityFromDb.Description = entityAfterMapping.Description;

      await _unitOfWork.CommitAsync();
      await _unitOfWork.CommitTransactionAsync();

      res.Data = entityAfterMapping.Id.ToString();
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
  public async Task<ApiResponse<string>> UpdateBothRoleWithRelatedPermissionsAsync(RoleForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repositoryRolePermissions = _unitOfWork.Repository<RolePermissions>();
    var repository = _unitOfWork.Repository<Role>();
    await _unitOfWork.BeginTransactionAsync();

    try
    {
      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var entityFromDb = await repository.GetAsync(request.Id.Value, include: x => x.Include(x => x.RolePermissions), tracking: true);

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

      await _unitOfWork.CommitAsync();
      await _unitOfWork.CommitTransactionAsync();

      res.Data = entityAfterMapping.Id.ToString();
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
  public async Task<ApiResponse<string>> UpdateRolePermissionsAsync(RoleForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var entityAfterMapping = request.ToEntity();

    var repositoryRolePermissions = _unitOfWork.Repository<RolePermissions>();
    var repository = _unitOfWork.Repository<Role>();
    await _unitOfWork.BeginTransactionAsync();

    try
    {

      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var entityFromDb = await repository.GetAsync(request.Id.Value, include: x => x.Include(x => x.RolePermissions), tracking: true);

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
      await _unitOfWork.CommitAsync();
      await _unitOfWork.CommitTransactionAsync();

      res.Data = entityAfterMapping.Id.ToString();
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
  public async Task<ApiResponse<string>> DeleteAsync(long id)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<Role>();

      var entityFromDB = await repository.GetAsync(id);
      if (entityFromDB == null)
        throw new Exception("Invalid user");

      repository.Delete(entityFromDB);
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

}

