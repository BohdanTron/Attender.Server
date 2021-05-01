using Attender.Server.API.Constants;
using Attender.Server.Application.Countries.Queries.GetCountry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Authorize(Policy = AuthConstants.Policy.RegisteredOnly)]
    [Produces(MediaTypeNames.Application.Json)]
    public class CountriesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CountryDto>> Get([FromQuery] string name)
        {
            var countries = await Mediator.Send(new GetCountriesQuery(name));
            return Ok(countries);
        }

        [HttpGet("closest")]
        public async Task<ActionResult<CountryDto>> GetClosestCountries([FromQuery] string code)
        {
            var countries = await Mediator.Send(new GetClosestCountries(code));
            return Ok(countries);
        }
    }
}
