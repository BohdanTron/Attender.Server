using Attender.Server.Application.Common.Auth.Dtos;
using Attender.Server.Application.Common.Helpers;
using FluentValidation;

namespace Attender.Server.Application.Common.Auth.Validators
{
    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator()
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
