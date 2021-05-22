using Attender.Server.API.Constants;
using Attender.Server.Application.Cities.Queries.GetCities;
using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// Gets list of cities
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CityDto>>> Get([FromQuery] GetCitiesQuery request)
        {
            return await Mediator.Send(request);
        }
    }
}
