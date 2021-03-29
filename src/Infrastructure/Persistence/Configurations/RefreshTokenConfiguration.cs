using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attender.Server.Infrastructure.Persistence.Configurations
{
    public partial class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> entity)
        {
            entity.Property(e => e.AddedDate).HasColumnType("datetime");

            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

            entity.Property(e => e.Value)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.AccessTokenId)
                .IsRequired()
                .IsUnicode(false);
        }
    }
}
