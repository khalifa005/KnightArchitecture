using FluentValidation;
using KH.Dto.Models.UserDto.Form;

namespace KH.Dto.Models.UserDto.Validation
{
  public class UserFormValidator : AbstractValidator<UserForm>
  {
    public UserFormValidator()
    {

      RuleFor(x => x.Email)
        .NotNull()
        .NotEmpty()
        .Length(1, 250)
        .WithMessage("please-enter-email");
    }
  }

}
