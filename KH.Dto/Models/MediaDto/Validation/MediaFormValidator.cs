using FluentValidation;
using KH.Dto.lookups.CityDto.Form;
using KH.Dto.Models.MediaDto.Form;

namespace KH.Dto.Models.MediaDto.Validation
{
  public class MediaFormValidator : AbstractValidator<MediaForm>
  {
    public MediaFormValidator()
    {

      RuleFor(x => x.Model)
        .NotNull()
        .WithMessage("please-enter-valid-model")
        .NotEmpty()
        .WithMessage("please-enter-valid-model")
        .Length(1, 250)
        .WithMessage("please-enter-valid-model");

      RuleFor(x => x.ModelId).Must(x => x.HasValue && x.Value < 10)
        .WithMessage("please-enter-valid-model-id");
    }
  }

}
