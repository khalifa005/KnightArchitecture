using FluentValidation;
using KH.Dto.lookups.RoleDto.Form;

namespace KH.Dto.lookups.RoleDto.Validation;

public class OtpVerificationValidator : AbstractValidator<RoleForm>
{
  public OtpVerificationValidator()
  {
    RuleFor(x => x.NameAr)
      .NotNull()
      .NotEmpty()
      .Length(1, 250);

    RuleFor(x => x.NameEn)
      .NotNull()
      .NotEmpty()
      .Length(1, 250);

    RuleFor(x => x.Description)
      .NotNull()
      .NotEmpty()
      .Length(1, 250)
      .WithMessage("please-enter-description");
  }
}
