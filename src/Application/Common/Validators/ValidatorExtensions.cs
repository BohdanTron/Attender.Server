using Attender.Server.Application.Common.Models;
using FluentValidation;

namespace Attender.Server.Application.Common.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error error)
        {
            return rule
                .Configure(config => config.CurrentValidator.Options.SetErrorMessage(error.Message))
                .Configure(config => config.CurrentValidator.Options.ErrorCode = error.Code);
        }
    }
}
