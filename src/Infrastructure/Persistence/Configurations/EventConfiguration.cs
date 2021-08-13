using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> entity)
        {
            entity.HasIndex(e => e.Name, "UC_Events_Name")
                .IsUnique();

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.Property(e => e.Description).IsUnicode(false);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Artist)
                .WithMany(p => p!.Events)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK_Events_Artists");

            entity.HasOne(d => d.Location)
                .WithMany(p => p!.Events)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_Events_Locations");

            entity.HasOne(d => d.SubCategory)
                .WithMany(p => p!.Events)
                .HasForeignKey(d => d.SubCategoryId)
                .HasConstraintName("FK_Events_SubCategories");

            entity.HasMany(e => e.Users)
                .WithMany(e => e.Events)
                .UsingEntity<Dictionary<string, object>>("UsersEvents",
                    e => e.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    e => e.HasOne<Event>().WithMany().HasForeignKey("EventId"),
                    e => e.ToTable("UsersEvents"));
        }
    }
}
