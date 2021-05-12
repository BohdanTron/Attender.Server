using Attender.Server.Application.Cities.Queries;
using Attender.Server.Application.Cities.Queries.GetPopularCities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    public class CitiesController  : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CityDto>>> Get([FromQuery] GetCitiesQuery request)
        {
            return await Mediator.Send(request);
        }
    }
}
