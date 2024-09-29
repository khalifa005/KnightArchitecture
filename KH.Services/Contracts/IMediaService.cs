using KH.Dto.Models.MediaDto.Form;
using KH.Dto.Models.MediaDto.Request;
using KH.Dto.Models.MediaDto.Response;

namespace KH.Services.Contracts
{
  public interface IMediaService
  {
    Task<ApiResponse<MediaResponse>> GetAsync(long id);
    Task<ApiResponse<PagedResponse<MediaResponse>>> GetListAsync(MediaRequest request);
    Task<ApiResponse<string>> AddAsync(MediaForm request);
    Task<ApiResponse<string>> AddListAsync(MediaForm request);
    Task<ApiResponse<string>> DeleteAsync(long id);
    Task<ApiResponse<MediaResponse>> DownloadAsync(long id);

  }
}
