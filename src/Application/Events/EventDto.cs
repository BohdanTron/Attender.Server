using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using AutoMapper;
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
        public decimal? LowestTicketPrice { get; set; }
        public string LocationName { get; init; } = string.Empty;
        public string ArtistName { get; init; } = string.Empty;
    }

    public record TicketDto
    {
        public int Id { get; init; }
        public decimal? Price { get; set; }
        public int EventId { get; init; }
    }
}
