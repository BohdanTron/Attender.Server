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
        }
    }
}
