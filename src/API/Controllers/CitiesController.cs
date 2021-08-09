using Attender.Server.API.Constants;
using Attender.Server.Application.Cities.Queries.GetCities;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Users.Commands.CreateUserCities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Authorize(Policy = AuthConstants.Policy.RegisteredOnly)]
    [Produces(MediaTypeNames.Application.Json)]
    public class CitiesController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public CitiesController(ICurrentUserService currentUserService) => 
            _currentUserService = currentUserService;

        /// <summary>
        /// Gets list of cities
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CityDto>>> Get([FromQuery] GetCitiesQuery request)
        {
            return await Mediator.Send(request);
        }

        /// <summary>
        /// Saves cities for the user and returns user's id
        /// </summary>
        /// <response code="201">Cities have been saved for the user. User's id returned</response>
        /// <response code="400">Given cities already applied for the user</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> SaveCities([FromBody] CreateUserCitiesDto request)
        {
            var userId = _currentUserService.UserId;

            var command = new CreateUserCitiesCommand(userId, request.CityIds);
            var result = await Mediator.Send(command);

            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Error);
        }
    }
}
