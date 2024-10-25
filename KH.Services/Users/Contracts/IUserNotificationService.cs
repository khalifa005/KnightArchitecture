namespace KH.Services.Users.Contracts;

public interface IUserNotificationService
{
  Task<ApiResponse<string>> SendUserWelcomeSmsAsync(User userEntity, CancellationToken cancellationToken);
  Task<ApiResponse<string>> SendUserWelcomeEmailAsync(User userEntity, DateTime? scheduleSendDateTime, CancellationToken cancellationToken);

}
