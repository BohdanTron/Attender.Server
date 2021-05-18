using Attender.Server.Application.Common.DTOs.Auth;
using Attender.Server.Application.Common.Helpers;
using FluentValidation;

namespace Attender.Server.Application.Common.Validators.Auth
{
    public class RegisterUserValidator : AbstractValidator<UserRegistrationInfoDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                    .WithError(Errors.User.UserNameRequired())
                .MaximumLength(25)
                    .WithError(Errors.User.UserNameTooLong(25));

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                    .WithError(Errors.PhoneNumber.Required())
                .Matches("^\\+[1-9]{1}[0-9]{7,14}$")
                    .WithError(Errors.PhoneNumber.Invalid());

            RuleFor(x => x.Email)
                .EmailAddress()
                    .WithError(Errors.User.EmailInvalid())
                .MaximumLength(50)
                    .WithError(Errors.User.EmailTooLong(50));
        }
    }
}
