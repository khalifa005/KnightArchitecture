using KH.Dto.Models.MediaDto.Request;
using KH.Dto.Models.MediaDto.Response;

namespace KH.Services.Media_s.Contracts;

public interface IMediaService
{
  Task<ApiResponse<MediaResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<MediaResponse>>> GetListAsync(MediaRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> AddAsync(CreateMediaRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> AddListAsync(CreateMediaRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<MediaResponse>> DownloadAsync(long id, CancellationToken cancellationToken);

}

