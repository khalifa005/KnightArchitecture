using KH.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Services.Features
{
  public class MediaService : IMediaService
  {
    public Task<ApiResponse<string>> AddAsync(UserForm request)
    {
      throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> AddListAsync(List<UserForm> request)
    {
      throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> DeleteAsync(long id)
    {
      throw new NotImplementedException();
    }

    public Task<ApiResponse<UserDetailsResponse>> GetAsync(long id)
    {
      throw new NotImplementedException();
    }

    public Task<ApiResponse<PagedResponse<UserListResponse>>> GetListAsync(UserFilterRequest request)
    {
      throw new NotImplementedException();
    }
  }
}
