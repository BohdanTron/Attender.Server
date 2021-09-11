using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class EventSectionConfiguration : IEntityTypeConfiguration<EventSection>
    {
        public void Configure(EntityTypeBuilder<EventSection> entity)
        {
            entity.HasKey(e => e.RowId);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);

            entity.HasOne(d => d.Language)
                .WithMany(p => p.EventSections)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK_EventSections_Languages");
        }
    }
}
