using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> entity)
        {
            entity.HasIndex(e => e.Name, "UC_Locations_Name")
                .IsUnique();

            entity.Property(e => e.Description).IsUnicode(false);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.City)
                .WithMany(p => p!.Locations)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Locations_Cities");

            entity.HasMany(e => e.Users)
                .WithMany(e => e.Locations)
                .UsingEntity<Dictionary<string, object>>("UsersLocations",
                    e => e.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    e => e.HasOne<Location>().WithMany().HasForeignKey("LocationId"),
                    e => e.ToTable("UsersLocations"));
        }
    }
}
