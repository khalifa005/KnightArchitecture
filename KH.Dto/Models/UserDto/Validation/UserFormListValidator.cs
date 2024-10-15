using FluentValidation;
using KH.Dto.Models.UserDto.Form;

namespace KH.Dto.Models.UserDto.Validation;

public class UserFormListValidator : AbstractValidator<List<UserForm>>
{
  public UserFormListValidator()
  {
    RuleForEach(x => x).SetValidator(new UserFormValidator()); // Validate each UserForm in the list
  }
}
