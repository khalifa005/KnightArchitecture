namespace KH.Services.Users.Contracts;

public interface IUserManagementService
{
  Task<ApiResponse<string>> AddAsync(CreateUserRequest request);
  Task<ApiResponse<string>> AddListAsync(List<CreateUserRequest> request);
  Task<ApiResponse<string>> UpdateAsync(CreateUserRequest request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<string>> ResetDepartmentsAsync(long id);
}
