using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Response;
using KH.Dto.Lookups.GroupDto.Request;

namespace KH.Services.Lookups.Groups.Contracts;

public interface IGroupService
{
  Task<ApiResponse<GroupResponse>> GetAsync(long id);
  Task<ApiResponse<PagedResponse<GroupListResponse>>> GetListAsync(GroupFilterRequest request);
  Task<ApiResponse<string>> AddAsync(CreateGroupRequest request);
  Task<ApiResponse<string>> UpdateAsync(CreateGroupRequest request);
  Task<ApiResponse<string>> DeleteAsync(long id);
}
