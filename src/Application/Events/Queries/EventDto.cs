using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace Attender.Server.Application.Events.Queries.GetUserEvents
{
    public record EventDto : IMapFrom<Event>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public DateTime Date { get; init; }
        public decimal LowestPrice { get; init; }
        public bool Promoted { get; init; }
        public string Location { get; init; } = string.Empty;
        public string Artist { get; init; } = string.Empty;
        public IEnumerable<TicketDto> Cities { get; init; } = new List<TicketDto>();

    }


    public record TicketDto
    {
        public int Id { get; init; }
        public decimal Price { get; init; }
        public DateTime? OrderedDate { get; init; }
        public int TicketsCount { get; init; }
        public int? UserId { get; init; }
        public int EventId { get; init; }
    }
}
