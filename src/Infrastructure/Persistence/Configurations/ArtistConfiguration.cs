using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class ArtistConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> entity)
        {
            entity.HasIndex(e => e.Name, "UC_Artists_Name")
                .IsUnique();

            entity.Property(e => e.Description).IsUnicode(false);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(e => e.Users)
              .WithMany(e => e.Artists)
              .UsingEntity<Dictionary<string, object>>("UsersArtists",
                  e => e.HasOne<User>().WithMany().HasForeignKey("UserId"),
                  e => e.HasOne<Artist>().WithMany().HasForeignKey("ArtistId"),
                  e => e.ToTable("UsersArtists"));
        }
    }
}
