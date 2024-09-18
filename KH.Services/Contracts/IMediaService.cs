
namespace KH.Services.Contracts
{
  public interface IMediaService
  {
    Task<ApiResponse<UserDetailsResponse>> GetAsync(long id);
    Task<ApiResponse<PagedResponse<UserListResponse>>> GetListAsync(UserFilterRequest request);
    Task<ApiResponse<string>> AddAsync(UserForm request);
    Task<ApiResponse<string>> AddListAsync(List<UserForm> request);
    Task<ApiResponse<string>> DeleteAsync(long id);
  }
}
