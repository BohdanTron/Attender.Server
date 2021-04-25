using Attender.Server.API.Common;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    //[Authorize(Policy = AuthConstants.Policy.RegisteredOnly)]
    public class UploadController : ApiControllerBase
    {
        private readonly IBlobService _blobService;

        public UploadController(IBlobService blobService)
        {
            _blobService = blobService;
        }


        /// <summary>
        /// Uploads user's avatar to the storage
        /// </summary>
        /// <response code="200">Avatar successfully uploaded</response>
        [HttpPost("avatar")]
        [ProducesResponseType(typeof(BlobInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult> Avatar([FromForm] UploadRequest request)
        {
            var file = request.File;
            var result = await _blobService.UploadAvatar(file.ContentType, file.OpenReadStream());

            return Ok(result);
        }
    }
}
