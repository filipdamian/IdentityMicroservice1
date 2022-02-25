using IdentityMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity.Configs
{
    public class IdentityRoleClaimConfig : IEntityTypeConfiguration<IdentityRoleClaim>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim> builder)
        {
            builder.ToTable(nameof(IdentityRoleClaim));
            builder.HasKey(rc => rc.Id);

            builder.Property(prop => prop.ClaimValue).IsRequired().HasMaxLength(50);
            builder.Property(prop => prop.ClaimType).IsRequired().HasMaxLength(50);


            builder.HasOne(r => r.Role)
                .WithMany(rc => rc.IdentityRoleClaims)
                .HasForeignKey(rc => rc.RoleId);

        }
    }
}
