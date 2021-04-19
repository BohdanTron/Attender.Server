using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Attender.Server.API.Common
{
    public class UploadRequest
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
