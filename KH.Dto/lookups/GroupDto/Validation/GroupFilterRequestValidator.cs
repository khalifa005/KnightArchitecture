using FluentValidation;
using KH.Dto.lookups.GroupDto.Request;

namespace KH.Dto.lookups.GroupDto
{
    public class CityFilterRequestValidator : AbstractValidator<GroupFilterRequest>
    {
        public CityFilterRequestValidator()
        {
            RuleFor(x => x.NameAr).NotNull().NotEmpty().Length(1, 250);
            RuleFor(x => x.NameEn).NotNull().NotEmpty().Length(1, 250);
        }
    }
    
}
