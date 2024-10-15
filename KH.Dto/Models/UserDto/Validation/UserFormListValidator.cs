using FluentValidation;
using KH.Dto.Models.UserDto.Request;

namespace KH.Dto.Models.UserDto.Validation;

public class UserFormListValidator : AbstractValidator<List<CreateUserRequest>>
{
  public UserFormListValidator()
  {
    RuleForEach(x => x).SetValidator(new UserFormValidator()); // Validate each UserForm in the list
  }
}
