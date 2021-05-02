using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Attender.Server.API.Requests.Upload
{
    public record UploadRequest
    {
        [Required]
        public IFormFile File { get; init; } = null!;
    }
}
