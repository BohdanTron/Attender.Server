using Attender.Server.API.Constants;
using Attender.Server.Application.Users.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Authorize(Policy = AuthConstants.Policy.RegisteredOnly)]
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<UserDto>> Get([FromQuery] string phoneNumber)
        {
            var user = await Mediator.Send(new GetUserQuery(phoneNumber));
            return user is not null ? Ok(user) : (ActionResult) NotFound();
        }
    }
}
