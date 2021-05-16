using Attender.Server.API.Constants;
using Attender.Server.Application.SubCategories.Queries;
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
    public class SubCategoriesController : ApiControllerBase
    {
        /// <summary>
        /// Get list of subcategories
        /// </summary>
        /// <param name="request">Category Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<SubCategoryDto>>> Get([FromQuery] GetSubCategoriesQuery request)
        {
            return await Mediator.Send(request);
        }
    }
}
