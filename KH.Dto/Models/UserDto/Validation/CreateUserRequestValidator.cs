using FluentValidation;
using KH.Dto.Models.UserDto.Request;

namespace KH.Dto.Models.UserDto.Validation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
  public CreateUserRequestValidator()
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
      .When(x => !x.Id.HasValue)  // Apply when creating a new user (no Id)
      .WithMessage("please-enter-username")
      .NotEmpty()
      .When(x => !x.Id.HasValue)  // Apply when creating a new user (no Id)
      .WithMessage("please-enter-username")
      .Length(1, 250)
      .When(x => !x.Id.HasValue)  // Apply when creating a new user (no Id)
      .WithMessage("please-enter-username");

    RuleFor(x => x.Password)
      .NotNull().When(x => !x.Id.HasValue)
      .WithMessage("please-enter-password")
      .NotEmpty().When(x => !x.Id.HasValue)
      .WithMessage("please-enter-password")
      .Length(1, 250)
      .When(x => !x.Id.HasValue)
      .WithMessage("please-enter-password");

    RuleFor(x => x.MobileNumber)
      .NotNull()
      .When(x => !x.Id.HasValue)
      .WithMessage("please-enter-mobile-number")
      .NotEmpty()
      .When(x => !x.Id.HasValue)
      .WithMessage("please-enter-mobile-number")
      .Length(1, 250)
      .When(x => !x.Id.HasValue)
      .WithMessage("please-enter-mobile-number");

    RuleFor(x => x.DepartmentId)
      .NotNull()
      .When(x => !x.Id.HasValue)
      .WithMessage("please-select-departmrnt");


    RuleFor(x => x.RoleIds)
      .Must(x => x.Count() > 0)
      .When(x => !x.Id.HasValue)
      .WithMessage("please-select-roles");
  }
}
