using FluentValidation;
using KH.Dto.lookups.DepartmentDto.Form;

namespace KH.Dto.lookups.DepartmentDto.Validation
{
  public class DepartmentFormValidator : AbstractValidator<DepartmentForm>
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

}
