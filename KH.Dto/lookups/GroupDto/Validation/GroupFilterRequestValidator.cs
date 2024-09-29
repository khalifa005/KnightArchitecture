using FluentValidation;
using KH.Dto.lookups.GroupDto.Request;

namespace KH.Dto.lookups.GroupDto.Validation;

public class GroupFilterRequestValidator : AbstractValidator<GroupFilterRequest>
{
  public GroupFilterRequestValidator()
  {
    RuleFor(x => x.NameAr).NotNull().NotEmpty().Length(1, 250);
    RuleFor(x => x.NameEn).NotNull().NotEmpty().Length(1, 250);
  }
}
