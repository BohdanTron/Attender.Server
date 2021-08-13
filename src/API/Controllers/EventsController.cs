using Attender.Server.API.Constants;
using Attender.Server.API.Requests;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Events.Queries.GetUserEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<PaginatedList<EventDto>>> Get([FromQuery] PaginationRequest request)
        {
            var query = new GetUserEventsQuery
            {
                UserId = _currentUserService.UserId,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber
            };

            return await Mediator.Send(query);
        }
    }
}
