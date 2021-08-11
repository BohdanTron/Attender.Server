using Attender.Server.API.Constants;
using Attender.Server.Application.Countries;
using Attender.Server.Application.Countries.Queries.GetClosestCountries;
using Attender.Server.Application.Countries.Queries.GetCountries;
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
    public class CountriesController : ApiControllerBase
    {
        /// <summary>
        /// Gets list of countries with their the most popular cities
        /// </summary>
        /// <response code="200">List of countries has been retrieved</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CountryDto>>> Get([FromQuery] GetCountriesQuery request)
        {
            return await Mediator.Send(request);
        }

        /// <summary>
        /// Gets closest countries with their the most popular cities
        /// </summary>
        /// <response code="200">List of the closest countries has been retrieved</response>
        [HttpGet("closest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CountryDto>>> GetClosest([FromQuery] GetClosestCountriesQuery request)
        {
            return await Mediator.Send(request);
        }
    }
}
