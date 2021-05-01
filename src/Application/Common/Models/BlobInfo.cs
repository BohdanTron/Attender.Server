using System.ComponentModel.DataAnnotations;

namespace Attender.Server.Application.Common.Models
{
    public record BlobInfo
    {
        [Required]
        public string Name { get; init; } = null!;

        [Required]
        public string Location { get; init; } = null!;
    }
}
