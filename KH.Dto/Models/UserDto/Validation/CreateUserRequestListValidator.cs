using FluentValidation;
using KH.Dto.Models.UserDto.Request;

namespace KH.Dto.Models.UserDto.Validation;

public class CreateUserRequestListValidator : AbstractValidator<List<CreateUserRequest>>
{
  public CreateUserRequestListValidator()
  {
    RuleForEach(x => x).SetValidator(new CreateUserRequestValidator()); // Validate each UserForm in the list
  }
}
