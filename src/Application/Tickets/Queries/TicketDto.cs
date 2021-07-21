using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;

namespace Attender.Server.Application.Tickets.Queries
{
    public record TicketDto : IMapFrom<Ticket>
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
    }
}
