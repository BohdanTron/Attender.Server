using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> entity)
        {
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Event)
                .WithMany(p => p!.Tickets)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_Tickets_Events");

            entity.HasOne(d => d.User)
                .WithMany(p => p!.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Users");
        }
    }
}
