using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> entity)
        {
            entity.HasIndex(e => e.Name, "UC_Countries_Name")
                .IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
