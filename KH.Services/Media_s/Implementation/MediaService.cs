using KH.BuildingBlocks.Files.Services;
using KH.Dto.Models.MediaDto.Request;
using KH.Dto.Models.MediaDto.Response;
using KH.Services.Media_s.Contracts;
using KH.Domain.Entities;

namespace KH.Services.Media_s.Implementation;

public class MediaService : IMediaService
{
  private readonly FileManagerService _fileManager;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IHostEnvironment _env;
  private readonly ILogger<MediaService> _logger;
  public MediaService(FileManagerService fileManager,
    IHostEnvironment env,
    ILogger<MediaService> logger,
    IUnitOfWork unitOfWork)
  {
    _logger = logger;
    _env = env;
    _fileManager = fileManager;
    _unitOfWork = unitOfWork;
  }

  public async Task<ApiResponse<MediaResponse>> GetAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<MediaResponse>? res = new ApiResponse<MediaResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<Media>();

    //light user query to make sure the user exist
    var entityFromDB = await repository.GetAsync(id, cancellationToken: cancellationToken);

    if (entityFromDB == null)
    {
      res.StatusCode = StatusCodes.Status400BadRequest;
      res.ErrorMessage = "invalid media";
    }
    MediaResponse entityResponse = new MediaResponse(entityFromDB);

    res.Data = entityResponse;
    return res;
  }

  public async Task<ApiResponse<PagedResponse<MediaResponse>>> GetListAsync(MediaRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<PagedResponse<MediaResponse>> apiResponse = new ApiResponse<PagedResponse<MediaResponse>>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<Media>();

    var pagedEntities = await repository.GetPagedWithProjectionAsync(
    pageNumber: request.PageIndex,
    pageSize: request.PageSize,
    filterExpression: u =>
    u.IsDeleted == false
    && u.ModelId == request.ModelId
    && u.Model == request.Model, // Filter by

    projectionExpression: u => new MediaResponse(u),
    orderBy: query => query.OrderBy(u => u.Id),  // Sort by Id
    tracking: false,  // Disable tracking for read-only queries
    cancellationToken: cancellationToken
);

    var entitiesResponses = pagedEntities.Select(x => x).ToList();

    var pagedResponse = new PagedResponse<MediaResponse>(
      entitiesResponses,
       pagedEntities.CurrentPage,
       pagedEntities.TotalPages,
       pagedEntities.PageSize,
       pagedEntities.TotalCount);

    apiResponse.Data = pagedResponse;

    return apiResponse;
  }

  public async Task<ApiResponse<string>> AddAsync(CreateMediaRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    bool isModelExists = Enum.IsDefined(typeof(ModelEnum), request.Model);

    await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      //all validation should be in fluent validation side
      if (request.File.Length <= 0)
        throw new Exception("file is required");

      if (request.ModelId == null)
        throw new Exception("Invalid Parameter");
      //-- Check Duplication
      //await this.CheckDuplictedUser();

      var fileRes = await _fileManager.Upload(request.File, string.Concat(request.Model, "_", request.ModelId));

      if (string.IsNullOrEmpty(fileRes.FilePath) || fileRes is null)
      {
        res.StatusCode = (int)HttpStatusCode.BadRequest;
        res.ErrorMessage = "empty-path";
        return res;
      }

      var mediaEntity = new Media();
      mediaEntity.Model = request.Model;
      mediaEntity.ModelId = request.ModelId.Value;

      mediaEntity.OrignalName = fileRes.OrignalFileName;
      mediaEntity.FileName = fileRes.GeneratedFileName;
      mediaEntity.Path = fileRes.FilePath;
      mediaEntity.Extention = fileRes.FileExtention;
      mediaEntity.ContentType = fileRes.ContentType;



      var repository = _unitOfWork.Repository<Media>();

      await repository.AddAsync(mediaEntity, cancellationToken: cancellationToken);
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

      await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);

      res.Data = mediaEntity.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken: cancellationToken);

      return ex.HandleException(res, _env, _logger);
    }
  }

  public async Task<ApiResponse<string>> AddListAsync(CreateMediaRequest request, CancellationToken cancellationToken)
  {
    bool isModelExists = Enum.IsDefined(typeof(ModelEnum), request.Model);

    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);
    await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      if (request.ModelId == null)
        throw new Exception("Invalid Parameter");

      //-- Check Duplication
      //await this.CheckDuplicted();

      var repository = _unitOfWork.Repository<Media>();

      var fileRes = await _fileManager.UploadMultiple(request.Files, string.Concat(request.Model, "_", request.ModelId));

      List<Media> mediaEntities = new List<Media>();

      foreach (var file in fileRes)
      {
        if (string.IsNullOrEmpty(file.FilePath) || fileRes is null)
        {
          res.StatusCode = (int)HttpStatusCode.BadRequest;
          res.ErrorMessage = "empty-path";
          return res;
        }

        var mediaEntity = new Media();

        mediaEntity.Model = request.Model;
        mediaEntity.ModelId = request.ModelId.Value;

        mediaEntity.OrignalName = file.OrignalFileName;
        mediaEntity.FileName = file.GeneratedFileName;
        mediaEntity.Path = file.FilePath;
        mediaEntity.Extention = file.FileExtention;

        mediaEntities.Add(mediaEntity);
      }

      await repository.AddRangeAsync(mediaEntities, cancellationToken: cancellationToken);
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);


      await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);

      res.Data = string.Join(",", mediaEntities.Select(x => x.Id));
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken: cancellationToken);

      return ex.HandleException(res, _env, _logger);

    }
  }

  public async Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<Media>();

      var entityDB = await repository.GetAsync(id, cancellationToken: cancellationToken);
      if (entityDB == null)
        throw new Exception("Invalid file");

      repository.Delete(entityDB);
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

      res.Data = entityDB.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {

      return ex.HandleException(res, _env, _logger);

    }
  }

  public async Task<ApiResponse<MediaResponse>> DownloadAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<MediaResponse>? res = new ApiResponse<MediaResponse>((int)HttpStatusCode.OK);


    if (id == null)
      throw new Exception("Invalid Parameter");

    var repository = _unitOfWork.Repository<Media>();

    //light user query to make sure the user exist
    var entityFromDBX = await repository.GetByExpressionAsync(x => x.Id == id, cancellationToken: cancellationToken);
    var entityFromDB = await repository.GetAsync(id, cancellationToken: cancellationToken);

    if (entityFromDB == null)
    {
      res.StatusCode = StatusCodes.Status400BadRequest;
      res.ErrorMessage = "invalid media";
    }

    MediaResponse entityResponse = new MediaResponse(entityFromDB);
    var physicalFileResponse = await _fileManager.Download(entityFromDB.Path);

    if (!physicalFileResponse.IsValid)
      throw new Exception("Invalid file");

    entityResponse.FileContentResult = physicalFileResponse.FileContentResult;

    res.Data = entityResponse;
    return res;
  }
}
