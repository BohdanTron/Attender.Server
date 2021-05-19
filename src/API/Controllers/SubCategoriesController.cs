using Attender.Server.API.Constants;
using Attender.Server.Application.SubCategories.Queries.GetSubCategories;
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
    public class SubCategoriesController : ApiControllerBase
    {
        /// <summary>
        /// Gets list of subcategories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<SubCategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> Get([FromQuery] GetSubCategoriesQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
