using FluentValidation;
using KH.Dto.lookups.CityDto.Form;

namespace KH.Dto.lookups.CityDto.Validation
{
  public class DepartmentFormValidator : AbstractValidator<CityForm>
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
