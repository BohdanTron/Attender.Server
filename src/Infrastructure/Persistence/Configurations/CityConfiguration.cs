using System.Collections.Generic;
using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> entity)
        {
            entity.HasIndex(e => e.Name, "UC_Cities_Name")
                .IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Country)
                .WithMany(p => p!.Cities)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Cities_Countries");

            entity.HasMany(e => e.Users)
                .WithMany(e => e.Cities)
                .UsingEntity<Dictionary<string, object>>("UsersCities",
                    e => e.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    e => e.HasOne<City>().WithMany().HasForeignKey("CityId"),
                    e => e.ToTable("UsersCities"));
        }
    }
}
