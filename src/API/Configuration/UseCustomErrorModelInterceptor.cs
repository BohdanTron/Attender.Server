using Attender.Server.Application.Common.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace Attender.Server.API.Configuration
{
    public class UseCustomErrorModelInterceptor : IValidatorInterceptor
    {
        public IValidationContext BeforeMvcValidation(ControllerContext controllerContext, IValidationContext commonContext)
        {
            return commonContext;
        }

        public ValidationResult AfterMvcValidation(
            ControllerContext controllerContext,
            IValidationContext commonContext,
            ValidationResult result)
        {
            var projection = result.Errors
                .Select(failure => new ValidationFailure(failure.PropertyName, SerializeFailure(failure)));

            return new ValidationResult(projection);
        }

        private static string SerializeFailure(ValidationFailure failure)
        {
            var error = new Error(failure.ErrorCode, failure.ErrorMessage);
            return JsonConvert.SerializeObject(error);
        }
    }
}
