using Attender.Server.API.Constants;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Users.Commands.CreateUserCities;
using Attender.Server.Application.Users.Commands.CreateUserSubCategories;
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
        private readonly ICurrentUserService _currentUserService;

        public UsersController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

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

        /// <summary>
        /// Saves subcategories for the user and returns user's id
        /// </summary>
        /// <response code="201">Subcategories have been saved for the user. User's id returned</response>
        /// <response code="400">Given subcategories already applied for the user</response>
        /// <response code="403">Access forbidden</response>
        [HttpPost("{userId:int}/sub-categories")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> SaveSubCategories([FromRoute] int userId,
            [FromBody] CreateUserSubCategoriesDto request)
        {
            var currentUserId = _currentUserService.UserId;

            if (currentUserId != userId) return Forbid();

            var command = new CreateUserSubCategoriesCommand(userId, request.SubCategoryIds);
            var result = await Mediator.Send(command);

            return result.Succeeded ? Created(string.Empty, result.Data) : BadRequest(result.Error);
        }

        /// <summary>
        /// Saves cities for the user and returns user's id
        /// </summary>
        /// <response code="201">Cities have been saved for the user. User's id returned</response>
        /// <response code="400">Given cities already applied for the user</response>
        /// <response code="403">Access forbidden</response>
        [HttpPost("{userId:int}/cities")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> SaveCities([FromRoute] int userId,
            [FromBody] CreateUserCitiesDto request)
        {
            var currentUserId = _currentUserService.UserId;

            if (currentUserId != userId) return Forbid();

            var command = new CreateUserCitiesCommand(userId, request.CityIds);
            var result = await Mediator.Send(command);

            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Error);
        }
    }
}
