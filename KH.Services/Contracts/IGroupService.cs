using KH.BuildingBlocks.Apis.Responses;
using KH.Dto.lookups.GroupDto.Form;
using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Response;

namespace KH.Services.Contracts;

public interface IGroupService
{
  Task<ApiResponse<GroupResponse>> GetAsync(long id);
  Task<ApiResponse<PagedResponse<GroupListResponse>>> GetListAsync(GroupFilterRequest request);
  Task<ApiResponse<string>> AddAsync(GroupForm request);
  Task<ApiResponse<string>> UpdateAsync(GroupForm request);
  Task<ApiResponse<string>> DeleteAsync(long id);
}
