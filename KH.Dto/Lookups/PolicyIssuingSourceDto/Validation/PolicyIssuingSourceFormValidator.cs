using FluentValidation;
using KH.Dto.lookups.PolicyIssuingSourceDto.Form;

namespace KH.Dto.lookups.PolicyIssuingSourceDto.Validation
{
  public class PolicyIssuingSourceFormValidator : AbstractValidator<PolicyIssuingSourceForm>
  {
    public PolicyIssuingSourceFormValidator()
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
