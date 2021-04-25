using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Common
{
    public record UploadRequest
    {
        [Required]
        public IFormFile File { get; init; } = null!;
    }
}
