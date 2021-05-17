using Attender.Server.API.Constants;
using Attender.Server.Application.Countries.Models;
using Attender.Server.Application.Countries.Queries.GetClosestCountries;
using Attender.Server.Application.Countries.Queries.GetCountries;
using Microsoft.AspNetCore.Authorization;
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
        /// Gets list of countries
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CountryDto>>> Get([FromQuery] GetCountriesQuery request)
        {
            return await Mediator.Send(request);
        }

        /// <summary>
        /// Gets closest countries and their cities
        /// </summary>
        [HttpGet("closest")]
        public async Task<ActionResult<List<CountryDto>>> GetClosestCountries([FromQuery] GetClosestCountriesQuery request)
        {
            return await Mediator.Send(request);
        }
    }
}
