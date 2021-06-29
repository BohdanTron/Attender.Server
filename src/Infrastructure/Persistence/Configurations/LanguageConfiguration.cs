using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> entity)
        {
            entity.HasIndex(e => e.Code, "UC_Languages_Code")
                .IsUnique();

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}
