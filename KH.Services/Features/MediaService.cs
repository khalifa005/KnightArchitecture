using KH.BuildingBlocks.Enums;
using KH.BuildingBlocks.Extentions.Files;
using KH.Dto.Models.MediaDto.Form;
using KH.Dto.Models.MediaDto.Request;
using KH.Dto.Models.MediaDto.Response;
using Microsoft.AspNetCore.Http;

namespace KH.Services.Features
{
  public class MediaService : IMediaService
  {
    private readonly FileManager _fileManager;
    private readonly IUnitOfWork _unitOfWork;
    public MediaService(FileManager fileManager, IUnitOfWork unitOfWork)
    {
      _fileManager = fileManager;
      _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<MediaResponse>> GetAsync(long id)
    {
      ApiResponse<MediaResponse>? res = new ApiResponse<MediaResponse>((int)HttpStatusCode.OK);

      var repository = _unitOfWork.Repository<Media>();

      //light user query to make sure the user exist
      var entityFromDB = await repository.GetAsync(id);

      if (entityFromDB == null)
      {
        res.StatusCode = (int)StatusCodes.Status400BadRequest;
        res.ErrorMessage = "invalid media";
      }
      MediaResponse entityResponse = new MediaResponse(entityFromDB);

      res.Data = entityResponse;
      return res;
    }

    public async Task<ApiResponse<PagedResponse<MediaResponse>>> GetListAsync(MediaRequest request)
    {
      ApiResponse<PagedResponse<MediaResponse>> apiResponse = new ApiResponse<PagedResponse<MediaResponse>>((int)HttpStatusCode.OK);

      var repository = _unitOfWork.Repository<Media>();

      var pagedEntities = await repository.GetPagedWithProjectionAsync<MediaResponse>(
      pageNumber: request.PageIndex,
      pageSize: request.PageSize,
      filterExpression: u =>
      u.IsDeleted == false
      && u.ModelId == request.ModelId
      && u.Model == request.Model, // Filter by

      projectionExpression: u => new MediaResponse(u),
      orderBy: query => query.OrderBy(u => u.Id),  // Sort by Id
      tracking: false  // Disable tracking for read-only queries
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

    public async Task<ApiResponse<string>> AddAsync(MediaForm request)
    {
      ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

      bool isModelExists = Enum.IsDefined(typeof(ModelEnum), request.Model);

      await _unitOfWork.BeginTransactionAsync();

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

        await repository.AddAsync(mediaEntity);
        await _unitOfWork.CommitAsync();

        await _unitOfWork.CommitTransactionAsync();

        res.Data = mediaEntity.Id.ToString();
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

    public async Task<ApiResponse<string>> AddListAsync(MediaForm request)
    {
      bool isModelExists = Enum.IsDefined(typeof(ModelEnum), request.Model);

      ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);
      await _unitOfWork.BeginTransactionAsync();

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

        await repository.AddRangeAsync(mediaEntities);
        await _unitOfWork.CommitAsync();


        await _unitOfWork.CommitTransactionAsync();

        res.Data = String.Join(",", mediaEntities.Select(x => x.Id));
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
        var repository = _unitOfWork.Repository<Media>();

        var entityDB = await repository.GetAsync(id);
        if (entityDB == null)
          throw new Exception("Invalid file");

        repository.Delete(entityDB);
        await _unitOfWork.CommitAsync();

        res.Data = entityDB.Id.ToString();
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

    public async Task<ApiResponse<MediaResponse>> DownloadAsync(long id)
    {
      ApiResponse<MediaResponse>? res = new ApiResponse<MediaResponse>((int)HttpStatusCode.OK);


      if (id == null)
        throw new Exception("Invalid Parameter");

      var repository = _unitOfWork.Repository<Media>();

      //light user query to make sure the user exist
      var entityFromDBX = await repository.GetByExpressionAsync(x => x.Id == id);
      var entityFromDB = await repository.GetAsync(id);

      if (entityFromDB == null)
      {
        res.StatusCode = (int)StatusCodes.Status400BadRequest;
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
}
