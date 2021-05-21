using System.Collections.Generic;
using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> entity)
        {
            entity.HasIndex(e => e.Name, "UC_SubCategories_Name")
                .IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Category)
                .WithMany(p => p!.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_SubCategories_Categories");

            entity.HasMany(e => e.Users)
                .WithMany(e => e.SubCategories)
                .UsingEntity<Dictionary<string, object>>("UsersSubCategories",
                    e => e.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    e => e.HasOne<SubCategory>().WithMany().HasForeignKey("SubCategoryId"),
                    e => e.ToTable("UsersSubCategories"));

        }
    }
}
