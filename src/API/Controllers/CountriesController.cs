using Attender.Server.Application.Users.Queries.GetCountry;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    public class CountriesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CountryDto>> Get([FromQuery] string name)
        {
            var countries = await Mediator.Send(new GetCountryQuery(name));
            return countries is not null ? Ok(countries) : (ActionResult) NotFound();
        }

        [HttpGet("closest")]
        public async Task<ActionResult<CountryDto>> GetClosestCountries([FromQuery] string code)
        {
            var countries = await Mediator.Send(new GetClosestCountries(code)); 
            return Ok(countries);
        }
    }
}
