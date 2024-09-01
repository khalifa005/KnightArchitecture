using FluentValidation;
using KH.Dto.lookups.CityDto.Request;

namespace KH.Dto.lookups.CityDto.Validation
{
    public class CityFilterRequestValidator : AbstractValidator<CityFilterRequest>
    {
        public CityFilterRequestValidator()
        {
            RuleFor(x => x.NameAr).NotNull().NotEmpty().Length(1, 250);
            RuleFor(x => x.NameEn).NotNull().NotEmpty().Length(1, 250);
        }
    }
    
}
