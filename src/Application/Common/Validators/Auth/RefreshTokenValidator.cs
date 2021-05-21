using Attender.Server.Application.Common.Dtos.Auth;
using Attender.Server.Application.Common.Helpers;
using FluentValidation;

namespace Attender.Server.Application.Common.Validators.Auth
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty()
                    .WithError(Errors.Auth.AccessTokenRequired());

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                    .WithError(Errors.Auth.RefreshTokenRequired());
        }
    }
}
