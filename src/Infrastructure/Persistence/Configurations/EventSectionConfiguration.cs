using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class EventSectionConfiguration : IEntityTypeConfiguration<EventSection>
    {
        public void Configure(EntityTypeBuilder<EventSection> entity)
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
        }
    }
}
