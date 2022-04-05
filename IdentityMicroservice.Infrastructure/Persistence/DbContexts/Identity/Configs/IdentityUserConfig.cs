using IdentityMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity.Configs
{
    public class IdentityUserConfig : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            builder.ToTable(nameof(IdentityUser));
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Username).IsRequired().HasMaxLength(50);
            builder.Property(prop => prop.Email).IsRequired().HasMaxLength(100);
            builder.Property(prop => prop.PasswordHash).IsRequired().HasMaxLength(500);
            builder.Property(prop => prop.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(prop => prop.PhoneNumberCountryPrefix).IsRequired().HasMaxLength(6);

            //define relations
            builder.HasMany(uc => uc.IdentityUserClaims).WithOne(u => u.User);
            builder.HasMany(utc => utc.IdentityUserTokenConfirmations).WithOne(u => u.User);
            builder.HasMany(ut => ut.IdentityUserTokens).WithOne(u => u.User);
            builder.HasMany(uel => uel.IdentityUserExternalLogins).WithOne(u => u.User);
            //
           // builder.HasMany(wot => wot.WOTypes).WithOne(u => u.IdentityUser);
            
        }
    }
}
