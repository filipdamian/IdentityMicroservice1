using IdentityMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity.Configs
{
    public class IdentityUserRoleConfig:IEntityTypeConfiguration<IdentityUserIdentityRole>
    {
     
        public void Configure(EntityTypeBuilder<IdentityUserIdentityRole> builder)
        {
            builder.ToTable(nameof(IdentityUserIdentityRole));
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne(ur => ur.IdentityUser)
                .WithMany(u => u.IdentityUserRoles)
                .HasForeignKey(ur => ur.UserId);

            builder.HasOne(ur => ur.IdentityRole)
                .WithMany(r => r.IdentityUserRoles)
                .HasForeignKey(ur => ur.RoleId);
        }
    }
}
