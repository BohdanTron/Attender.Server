namespace Attender.Server.Application.Common.Models
{
    public record BlobInfo
    {
        public BlobInfo() { }

        public BlobInfo(string name, string location)
            => (Name, Location) = (name, location);

        public string Name { get; init; } = string.Empty;
        public string Location { get; init; } = string.Empty;
    }
}
