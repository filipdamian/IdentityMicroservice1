using IdentityMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity.Configs
{
    public class IdentityUserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim> builder)
        {
            builder.ToTable(nameof(IdentityUserClaim));
            builder.HasKey(ucl => ucl.Id);

            builder.Property(prop => prop.ClaimType).IsRequired().HasMaxLength(50);
            builder.Property(prop => prop.ClaimValue).IsRequired().HasMaxLength(50);

            builder.HasOne(u => u.User)
                .WithMany(ucl => ucl.IdentityUserClaims)
                .HasForeignKey(u => u.UserId);

        }
    }
}
