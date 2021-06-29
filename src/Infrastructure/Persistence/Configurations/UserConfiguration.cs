using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasIndex(e => e.Email, "IDX_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.PhoneNumber, "UC_Users_PhoneNumber")
                .IsUnique();

            entity.HasIndex(e => e.UserName, "UC_Users_UserName")
                .IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Role)
                .WithMany(p => p!.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_Roles");

            entity.HasOne(d => d.Language)
                .WithMany(p => p!.Users)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK_Users_Languages");
        }
    }
}
