using Attender.Server.API.Constants;
using Attender.Server.Application.Categories;
using Attender.Server.Application.Categories.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Authorize(Policy = AuthConstants.Policy.RegisteredOnly)]
    [Produces(MediaTypeNames.Application.Json)]
    public class CategoriesController : ApiControllerBase
    {
        /// <summary>
        /// Get categories with appropriate subcategories by category Id
        /// </summary>
        /// <param name="request">category Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> Get([FromQuery] GetCategoriesWithSubCategoriesQuery request)
        {
            return await Mediator.Send(request);
        }
    }
}
