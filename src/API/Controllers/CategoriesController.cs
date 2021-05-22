using Attender.Server.API.Constants;
using Attender.Server.Application.Categories.Queries.GetCategories;
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
    public class CategoriesController : ApiControllerBase
    {
        /// <summary>
        /// Get categories with appropriate subcategories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryDto>>> Get([FromQuery] GetCategoriesQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
