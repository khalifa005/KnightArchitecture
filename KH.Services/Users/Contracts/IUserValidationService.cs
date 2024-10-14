namespace KH.Services.Users.Contracts;

public interface IUserValidationService
{
  Task<bool> IsThereMatchedUserAsync(string email, string username);
  Task<bool> IsThereMatchedUserWithTheSamePhoneNumberAsync(string phoneNumber);
}
