using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Attender.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender? _mediator;

        protected ISender Mediator => (_mediator ??= HttpContext.RequestServices.GetService<ISender>()) ??
                                      throw new InvalidOperationException();
    }
}
