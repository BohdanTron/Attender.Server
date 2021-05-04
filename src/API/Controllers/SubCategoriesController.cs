using Attender.Server.Application.SubCategories.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    public class SubCategoriesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<SubCategoryDto>>> Get([FromQuery] GetSubCategoriesQuery request)
        {
            return await Mediator.Send(request);
        }
    }
}
