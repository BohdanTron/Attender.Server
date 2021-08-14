using Attender.Server.API.Constants;
using Attender.Server.API.Requests;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Events.Queries.GetUserEvents;
using Attender.Server.Application.EventSections.Queries.GetEventSections;
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
        [HttpGet("section/{sectionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedList<EventDto>>> Get(int sectionId, [FromQuery] PaginationRequest request)
        {
            var query = new GetUserEventsQuery
            {
                UserId = _currentUserService.UserId,
                SectionId = sectionId,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber
            };

            return await Mediator.Send(query);
        }

        /// <summary>
        /// Gets list of event sections
        /// </summary>
        /// <response code="200">List of event sections has been retrieved</response>
        [HttpGet("sections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EventSectionDto>>> GetSections()
        {
            return await Mediator.Send(new GetEventSectionsQuery());
        }
    }
}
