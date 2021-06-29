using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class CategoryDescriptionConfiguration : IEntityTypeConfiguration<CategoryDescription>
    {
        public void Configure(EntityTypeBuilder<CategoryDescription> entity)
        {
            entity.Property(e => e.Text)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Category)
                .WithMany(p => p!.CategoryDescriptions)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_CategoryDescriptions_Categories");

            entity.HasOne(d => d.Language)
                .WithMany(p => p!.CategoryDescriptions)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK_CategoryDescriptions_Languages");
        }
    }
}
