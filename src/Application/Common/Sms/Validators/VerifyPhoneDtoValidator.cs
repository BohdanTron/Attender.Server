using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Sms.Dtos;
using FluentValidation;

namespace Attender.Server.Application.Common.Sms.Validators
{
    public class VerifyPhoneDtoValidator : AbstractValidator<PhoneVerificationDto>
    {
        public VerifyPhoneDtoValidator()
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
