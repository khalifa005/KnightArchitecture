using FluentValidation;
using KH.Dto.lookups.CityDto.Form;
using KH.Dto.Models.MediaDto.Form;
using KH.Dto.Models.UserDto.Form;

namespace KH.Dto.Models.UserDto.Validation
{
  public class MediaFormValidator : AbstractValidator<MediaForm>
  {
    public MediaFormValidator()
    {

      RuleFor(x => x.Model)
        .NotNull()
        .NotEmpty()
        .Length(1, 250)
        .WithMessage("please-enter-valid-model-id");

      RuleFor(x => x.ModelId).Must(x=> x.HasValue && x.Value < 10)
        .WithMessage("please-enter-valid-model-id");
    }
  }

}
