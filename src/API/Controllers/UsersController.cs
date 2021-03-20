using Attender.Server.Application.Users.Queries.GetUser;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            return await Mediator.Send(new GetUserQuery(id));
        }
    }
}
