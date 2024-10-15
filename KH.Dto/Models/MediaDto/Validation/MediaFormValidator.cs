using FluentValidation;
using KH.Dto.Models.MediaDto.Request;

namespace KH.Dto.Models.MediaDto.Validation;

public class MediaFormValidator : AbstractValidator<CreateMediaRequest>
{
  public MediaFormValidator()
  {

    RuleFor(x => x.Model)
      .NotNull()
      .WithMessage("please-enter-valid-model")
      .NotEmpty()
      .WithMessage("please-enter-valid-model");

    RuleFor(x => x.ModelId).NotNull()
      .WithMessage("please-enter-valid-model-id");
  }
}
