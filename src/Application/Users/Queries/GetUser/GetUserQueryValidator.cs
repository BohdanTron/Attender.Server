using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Validators;
using FluentValidation;

namespace Attender.Server.Application.Users.Queries.GetUser
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithError(Errors.PhoneNumber.Required());
        }
    }
}
