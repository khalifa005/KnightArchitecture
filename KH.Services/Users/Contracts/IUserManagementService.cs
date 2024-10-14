namespace KH.Services.Users.Contracts;

public interface IUserManagementService
{
  Task<ApiResponse<string>> AddAsync(UserForm request);
  Task<ApiResponse<string>> AddListAsync(List<UserForm> request);
  Task<ApiResponse<string>> UpdateAsync(UserForm request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<string>> ResetDepartmentsAsync(long id);
}
