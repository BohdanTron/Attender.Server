using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    public class UploadController : ApiControllerBase
    {
        private readonly IBlobService _blobService;

        public UploadController(IBlobService blobService) => 
            _blobService = blobService;

        /// <summary>
        /// Uploads user's avatar to the storage
        /// </summary>
        /// <response code="200">Avatar successfully uploaded</response>
        [HttpPost("avatar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BlobInfo>> Avatar([Required] IFormFile file)
        {
            var result = await _blobService.UploadAvatar(file.ContentType, file.OpenReadStream());
            return Ok(result);
        }
    }
}
