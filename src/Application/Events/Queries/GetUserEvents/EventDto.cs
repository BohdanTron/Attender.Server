using System;

namespace Attender.Server.Application.Events.Queries.GetUserEvents
{
    public record EventDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public DateTime Date { get; init; }
        public decimal LowestPrice { get; init; }
        public string Location { get; init; } = string.Empty;
        public string Artist { get; init; } = string.Empty;
    }
}
