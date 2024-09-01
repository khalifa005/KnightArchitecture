using FluentValidation;
using KH.Dto.Models.AuthenticationDto.Request;

namespace KH.Dto.Models.OtpVerificationDto.Validation
{
  public class OtpVerificationRequestValidator : AbstractValidator<OtpVerificationRequest>
  {
    public OtpVerificationRequestValidator()
    {

      RuleFor(x => x.OtpCode)
        .NotNull()
        .NotEmpty()
        .Length(1, 250)
        .WithMessage("please-enter-otp-code");
    }
  }

}
