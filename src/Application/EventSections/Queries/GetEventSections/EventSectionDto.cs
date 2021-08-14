using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;

namespace Attender.Server.Application.EventSections.Queries.GetEventSections
{
    public record EventSectionDto : IMapFrom<EventSection>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
