namespace Attender.Server.Application.Common.Models
{
    public record BlobInfo
    {
        public string Name { get; init; } = null!;
        public string Location { get; init; } = null!;
    }
}
