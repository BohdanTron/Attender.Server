using Attender.Server.API.Constants;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Events.Queries.GetUserEvents;
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
    public class EventsController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public EventsController(ICurrentUserService currentUserService) =>
            _currentUserService = currentUserService;

        /// <summary>
        /// Gets list of events for specific user by their preferred locations and subcategories
        /// </summary>
        /// <response code="200">List of events has been retrieved</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EventDto>>> Get()
        {
            var userId = _currentUserService.UserId;
            return await Mediator.Send(new GetUserEventsQuery(userId));
        }
    }
}
