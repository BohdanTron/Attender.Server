using Attender.Server.Application.Events;
using Attender.Server.Application.Events.Queries.GetEventsForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    //[Authorize(Policy = AuthConstants.Policy.RegisteredOnly)]
    [Produces(MediaTypeNames.Application.Json)]
    public class EventsController :ApiControllerBase
    {
        /// <summary>
        /// Gets list of events for specific  user by his preferred locations and subcategories
        /// </summary>
        /// <response code="200">List of countries has been retrieved</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EventDto>>> Get([FromQuery] GetUserEventsQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
