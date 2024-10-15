using FluentValidation;
using KH.Dto.Models.AuthenticationDto.Request;

namespace KH.Dto.Models.UserDto.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
  public LoginRequestValidator()
  {

    RuleFor(x => x.Username)
      .NotNull()
      .WithMessage("please-enter-username")
      .NotEmpty()
      .WithMessage("please-enter-username")
      .Length(1, 250)
      .WithMessage("please-enter-username");


    RuleFor(x => x.Password)
      .NotNull()
      .WithMessage("please-enter-password")
      .NotEmpty()
      .WithMessage("please-enter-password")
      .Length(1, 250)
      .WithMessage("please-enter-password");
  }
}
