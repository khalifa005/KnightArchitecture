using FluentValidation;
using KH.Dto.Models.CustomerDto.Request;

namespace KH.Dto.Models.CustomerDto.Validation;

public class CustomerFormValidator : AbstractValidator<CreateCustomerRequest>
{
  public CustomerFormValidator()
  {
    RuleFor(x => x.Email)
      .NotNull()
      .NotEmpty()
      .Length(1, 250)
      .WithMessage("please-enter-email");
  }
}
