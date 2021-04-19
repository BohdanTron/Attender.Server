using Attender.Server.API.Common;
using Attender.Server.API.Constants;
using Attender.Server.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("avatar")]
        public async Task<ActionResult> Avatar([FromForm] UploadRequest request)
        {
            var file = request.File;
            var url = await _blobService.UploadAvatar(file.ContentType, file.OpenReadStream());

            return Ok(url);
        }
    }
}
