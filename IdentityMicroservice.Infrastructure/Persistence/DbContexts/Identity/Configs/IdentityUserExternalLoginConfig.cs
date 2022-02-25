using IdentityMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity.Configs
{
    public class IdentityUserExternalLoginConfig : IEntityTypeConfiguration<IdentityUserExternalLogin>
    {
        public void Configure(EntityTypeBuilder<IdentityUserExternalLogin> builder)
        {
            builder.ToTable(nameof(IdentityUserExternalLogin));
            builder.HasKey(prop => prop.Id);

            //configure string properties
            builder.Property(prop => prop.LoginProvider).IsRequired().HasMaxLength(100);
            builder.Property(prop => prop.ProviderKey).IsRequired().HasMaxLength(500);
            builder.Property(prop => prop.ProviderDisplayName).IsRequired().HasMaxLength(50);


            builder.HasOne(u => u.User)
                .WithMany(uel => uel.IdentityUserExternalLogins)
                .HasForeignKey(uel => uel.UserId);
        }
    }
}
