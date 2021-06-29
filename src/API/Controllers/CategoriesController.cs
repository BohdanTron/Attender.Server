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
    [Route("api/{languageCode}/categories")]
    public class CategoriesController : ApiControllerBase
    {
        /// <summary>
        /// Gets categories with appropriate subcategories
        /// </summary>
        /// <response code="200">List of categories has been retrieved</response>
        /// <response code="404">Requested language code is not supported</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryDto>>> Get(string languageCode)
        {
            var result = await Mediator.Send(new GetCategoriesQuery(languageCode));
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Error);
        }
    }
}
