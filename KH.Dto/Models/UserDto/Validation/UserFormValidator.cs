using FluentValidation;
using KH.Dto.Models.AuthenticationDto.Request;
using KH.Dto.Models.UserDto.Form;

namespace KH.Dto.Models.UserDto.Validation;

public class UserFormValidator : AbstractValidator<UserForm>
{
  public UserFormValidator()
  {

    RuleFor(x => x.FirstName)
      .NotNull()
      .WithMessage("please-enter-first-name")
      .NotEmpty()
      .WithMessage("please-enter-first-name")
      .Length(1, 250)
      .WithMessage("please-enter-first-name");

    RuleFor(x => x.Email)
      .NotNull()
      .WithMessage("please-enter-email")
      .NotEmpty()
      .WithMessage("please-enter-email")
      .Length(1, 250)
      .WithMessage("please-enter-email");

    RuleFor(x => x.Username)
      .NotNull()
      .WithMessage("please-enter-username")
      .NotEmpty()
      .WithMessage("please-enter-username")
      .Length(1, 250)
      .WithMessage("please-enter-username");


    RuleFor(x => x.Password)
      .NotNull().When(x=> !x.Id.HasValue)
      .WithMessage("please-enter-password")
      .NotEmpty().When(x => !x.Id.HasValue)
      .WithMessage("please-enter-password")
      .Length(1, 250)
      .When(x => !x.Id.HasValue)
      .WithMessage("please-enter-password");

    RuleFor(x => x.Username)
      .NotNull()
      .WithMessage("please-enter-username")
      .NotEmpty()
      .WithMessage("please-enter-username")
      .Length(1, 250)
      .WithMessage("please-enter-username");

    RuleFor(x => x.MobileNumber)
      .NotNull()
      .WithMessage("please-enter-mobile-number")
      .NotEmpty()
      .WithMessage("please-enter-mobile-number")
      .Length(1, 250)
      .WithMessage("please-enter-mobile-number");

    RuleFor(x => x.DepartmentId)
      .NotNull()
      .WithMessage("please-select-departmrnt");


    RuleFor(x => x.RoleIds)
      .Must(x => x.Count() > 0)
      .WithMessage("please-select-roles");
  }
}

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
