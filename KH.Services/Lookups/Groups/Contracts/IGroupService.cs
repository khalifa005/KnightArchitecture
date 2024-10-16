using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Response;
using KH.Dto.Lookups.GroupDto.Request;

namespace KH.Services.Lookups.Groups.Contracts;

public interface IGroupService
{
  Task<ApiResponse<GroupResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<GroupListResponse>>> GetListAsync(GroupFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> AddAsync(CreateGroupRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> UpdateAsync(CreateGroupRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken);
}