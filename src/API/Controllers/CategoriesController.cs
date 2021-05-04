using Attender.Server.Application.Categories;
using Attender.Server.Application.Categories.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> Get()
        {
            return await Mediator.Send(new GetCategoriesQuery());
        }
    }
}
