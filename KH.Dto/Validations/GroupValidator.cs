using KH.Dto.Models.lookups;
using FluentValidation;

namespace CA.ViewModels.Validations
{
    public class GroupValidator : AbstractValidator<GroupResponseDto>
    {
        public GroupValidator()
        {
            RuleFor(x => x.NameAr).NotNull().NotEmpty().Length(1, 250);
            RuleFor(x => x.NameEn).NotNull().NotEmpty().Length(1, 250);
        }
    }
    
}
