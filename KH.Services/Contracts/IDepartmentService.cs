using KH.BuildingBlocks.Apis.Responses;
using KH.Dto.lookups.DepartmentDto.Form;
using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;

namespace KH.Services.Contracts;

public interface IDepartmentService
{
  Task<ApiResponse<DepartmentResponse>> GetAsync(long id);
  Task<ApiResponse<PagedResponse<DepartmentListResponse>>> GetListAsync(DepartmentFilterRequest request);
  Task<ApiResponse<string>> AddAsync(DepartmentForm request);
  Task<ApiResponse<string>> UpdateAsync(DepartmentForm request);
  Task<ApiResponse<string>> DeleteAsync(long id);
}
