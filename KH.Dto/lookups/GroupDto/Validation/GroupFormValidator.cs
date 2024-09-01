using FluentValidation;
using KH.Dto.lookups.GroupDto.Form;

namespace KH.Dto.lookups.Group.Validation
{
  public class CityFormValidator : AbstractValidator<GroupForm>
  {
    public CityFormValidator()
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
