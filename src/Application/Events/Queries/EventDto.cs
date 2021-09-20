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
        public IEnumerable<TicketDto> Cities { get; set; } = new List<TicketDto>();

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<Event, EventDto>()
        //        .ForMember(d => d., opt => opt.Ignore());
        //}
    }

    public record TicketDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime? OrderedDate { get; set; }
        public int TicketsCount { get; set; }
        public int? UserId { get; set; }
        public int EventId { get; set; }
    }
}
