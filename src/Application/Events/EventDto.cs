using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attender.Server.Application.Events
{
    public record EventDto : IMapFrom<Event>
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
        public DateTime Date { get; init; }
        public int LocationId { get; init; }
        public int ArtistId { get; init; }
        public int SubCategoryId { get; init; }
    }
}
