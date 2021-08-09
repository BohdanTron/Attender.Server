using Attender.Server.API.Constants;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Users.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Authorize(Policy = AuthConstants.Policy.RegisteredOnly)]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ApiControllerBase
    {
        /// <summary>
        /// Gets user by given username
        /// </summary>
        /// <response code="200">User has been retrieved</response>
        /// <response code="404">User doesn't exist</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Get([FromQuery] GetUserQuery request)
        {
            var result = await Mediator.Send(request);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Error);
        }
    }
}
