using IdentityMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity.Configs
{
    public class IdentityUserTokenConfirmationConfig : IEntityTypeConfiguration<IdentityUserTokenConfirmation>
    {
        public void Configure(EntityTypeBuilder<IdentityUserTokenConfirmation> builder)
        {
            builder.ToTable(nameof(IdentityUserTokenConfirmation));
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.ConfirmationToken).IsRequired().HasMaxLength(500);

            builder.HasOne(prop => prop.User)
                .WithMany(prop => prop.IdentityUserTokenConfirmations)
                .HasForeignKey(prop => prop.UserId);
        }
    }
}
