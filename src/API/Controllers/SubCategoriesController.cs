using Attender.Server.API.Constants;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.SubCategories.Commands.CreateUserSubCategories;
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
        private readonly ICurrentUserService _currentUserService;

        public SubCategoriesController(ICurrentUserService currentUserService) =>
            _currentUserService = currentUserService;

        /// <summary>
        /// Searches subcategories by name in scope of category, retrieves all subcategories from category if name is null
        /// </summary>
        /// <response code="200">List of subcategories has been retrieved</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SubCategoryDto>>> Get([FromQuery] GetSubCategoriesQuery request)
        {
            return await Mediator.Send(request);
        }

        /// <summary>
        /// Saves subcategories for the user and returns user's id
        /// </summary>
        /// <response code="201">Subcategories have been saved for the user. User's id returned</response>
        /// <response code="400">Given subcategories already applied for the user</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Save([FromBody] CreateUserSubCategoriesDto request)
        {
            var command = new CreateUserSubCategoriesCommand
            {
                UserId = _currentUserService.UserId,
                Categories = request.Categories,
                BindAllCategories = request.BindAllCategories
            };
            var result = await Mediator.Send(command);

            return result.Succeeded
                ? Created(string.Empty, result.Data)
                : BadRequest(result.Error);
        }
    }
}
