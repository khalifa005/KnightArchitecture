using KH.Domain.Entities.lookups;
using KH.Dto.lookups.DepartmentDto.Form;
using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;

public class DepartmentService : IDepartmentService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IServiceProvider _serviceProvider;
  private readonly IMapper _mapper;

  public DepartmentService(
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider,
    IMapper mapper)
  {
    _unitOfWork = unitOfWork;
    _serviceProvider = serviceProvider;
    _mapper = mapper;
  }

  public async Task<ApiResponse<DepartmentResponse>> GetAsync(long id)
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
  public async Task<ApiResponse<PagedResponse<DepartmentListResponse>>> GetListAsync(DepartmentFilterRequest request)
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


    var entityListResponses = pagedEntities.Select(x => x).ToList();

    var pagedResponse = new PagedResponse<DepartmentListResponse>(
      entityListResponses,
       pagedEntities.CurrentPage,
       pagedEntities.TotalPages,
       pagedEntities.PageSize,
       pagedEntities.TotalCount);

    var apiResponse = new ApiResponse<PagedResponse<DepartmentListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
  public async Task<ApiResponse<string>> AddAsync(DepartmentForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync();

    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      //all validation should be in fluent validation side
      if (request.NameEn.IsNullOrEmpty())
        throw new Exception("NameEn is required");

      var entity = request.ToEntity();

      var repository = _unitOfWork.Repository<Department>();

      await repository.AddAsync(entity);
      //await _unitOfWork.CommitWithAuditingAsync("26");
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
  public async Task<ApiResponse<string>> UpdateAsync(DepartmentForm request)
  {
    //define our api res 
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    //auto mapper
    var entityAfterMapping = request.ToEntity();

    var repository = _unitOfWork.Repository<Department>();
    await _unitOfWork.BeginTransactionAsync();

    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var entityFromDb = await repository.GetAsync(request.Id.Value, tracking: true);

      if (entityFromDb == null)
        throw new Exception("Invalid Parameter");

      //cleaner way
      //var oldProduct = await _context.Product.FindAsync(id);
      //_context.Entry(oldProduct).CurrentValues.SetValues(product);

      //Add the new props here ..etc
      entityFromDb.NameAr = entityAfterMapping.NameAr;
      entityFromDb.NameEn = entityAfterMapping.NameEn;
      entityFromDb.Description = entityAfterMapping.Description;

      //repository.Update(entityFromDb);
      //repository.UpdateX(entityFromDb, entityAfterMapping);
      await _unitOfWork.CommitAsync();
      //await _unitOfWork.CommitWithAuditingAsync("26");

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
      var repository = _unitOfWork.Repository<Department>();

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

