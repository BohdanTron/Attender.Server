using Attender.Server.API.Constants;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.SubCategories.Queries.GetSubCategories;
using Attender.Server.Application.Users.Commands.CreateUserSubCategories;
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
        /// Gets list of subcategories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SubCategoryDto>>> Get([FromQuery] GetSubCategoriesQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Saves subcategories for the user and returns user's id
        /// </summary>
        /// <response code="201">Subcategories have been saved for the user. User's id returned</response>
        /// <response code="400">Given subcategories already applied for the user</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> SaveSubCategories([FromBody] CreateUserSubCategoriesDto request)
        {
            var userId = _currentUserService.UserId;

            var command = new CreateUserSubCategoriesCommand(userId, request.SubCategoryIds);
            var result = await Mediator.Send(command);

            return result.Succeeded ? Created(string.Empty, result.Data) : BadRequest(result.Error);
        }
    }
}
