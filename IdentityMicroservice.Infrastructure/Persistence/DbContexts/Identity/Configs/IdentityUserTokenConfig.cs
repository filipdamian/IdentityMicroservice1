using IdentityMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity.Configs
{
    public class IdentityUserTokenConfig : IEntityTypeConfiguration<IdentityUserToken>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken> builder)
        {
            builder.ToTable(nameof(IdentityUserToken));
            builder.HasKey(ut => ut.Id);

            builder.Property(prop => prop.RefreshTokenValue).HasMaxLength(500);
            builder.Property(prop => prop.TokenValue).IsRequired().HasMaxLength(500);

            builder.HasOne(u => u.User)
                .WithMany(ut => ut.IdentityUserTokens)
                .HasForeignKey(ut => ut.UserId);
        }
    }
}
