using FluentValidation;
using KH.Dto.Lookups.GroupDto.Request;

namespace KH.Dto.Lookups.GroupDto.Validation;

public class GroupFormValidator : AbstractValidator<CreateGroupRequest>
{
  public GroupFormValidator()
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
