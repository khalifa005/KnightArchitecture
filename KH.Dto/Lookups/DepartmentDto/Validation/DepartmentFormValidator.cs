using FluentValidation;
using KH.Dto.Lookups.DepartmentDto.Request;

namespace KH.Dto.lookups.DepartmentDto.Validation;

public class DepartmentFormValidator : AbstractValidator<CreateDepartmentRequest>
{
  public DepartmentFormValidator()
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
