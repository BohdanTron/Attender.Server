using Attender.Server.Application.Common.DTOs.Sms;
using Attender.Server.Application.Common.Helpers;
using FluentValidation;

namespace Attender.Server.Application.Common.Validators.Sms
{
    public class VerifyPhoneValidator : AbstractValidator<PhoneVerificationDto>
    {
        public VerifyPhoneValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                    .WithError(Errors.Sms.VerificationCodeRequired());

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                    .WithError(Errors.PhoneNumber.Required())
                .Matches("^\\+[1-9]{1}[0-9]{7,14}$")
                    .WithError(Errors.PhoneNumber.Invalid());
        }
    }
}
