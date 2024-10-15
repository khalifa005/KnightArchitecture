using KH.Dto.Models.MediaDto.Request;
using KH.Dto.Models.MediaDto.Response;

namespace KH.Services.Media_s.Contracts;

public interface IMediaService
{
  Task<ApiResponse<MediaResponse>> GetAsync(long id);
  Task<ApiResponse<PagedResponse<MediaResponse>>> GetListAsync(MediaRequest request);
  Task<ApiResponse<string>> AddAsync(CreateMediaRequest request);
  Task<ApiResponse<string>> AddListAsync(CreateMediaRequest request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<MediaResponse>> DownloadAsync(long id);

}

