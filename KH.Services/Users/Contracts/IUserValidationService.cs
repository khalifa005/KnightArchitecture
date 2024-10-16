namespace KH.Services.Users.Contracts;

public interface IUserValidationService
{
  Task<bool> IsThereMatchedUserAsync(string email, string username, CancellationToken cancellationToken);
  Task<bool> IsThereMatchedUserWithTheSamePhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
}
