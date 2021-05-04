using Attender.Server.Application.Cities.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    public class CitiesController  : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CityDto>> Get([FromQuery] string name)
        {
            var countries = await Mediator.Send(new GetCitiesQuery(name));
            return countries is not null ? Ok(countries) : (ActionResult)NotFound();
        }
    }
}
