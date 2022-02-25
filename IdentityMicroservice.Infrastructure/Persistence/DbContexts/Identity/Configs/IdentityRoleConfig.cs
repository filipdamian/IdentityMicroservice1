using IdentityMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity.Configs
{
    public class IdentityRoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.ToTable(nameof(IdentityRole));
            builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
            builder.HasKey(r => r.Id);
            builder.HasMany(rc => rc.IdentityRoleClaims).WithOne(r => r.Role).HasForeignKey(rc=>rc.RoleId);
            
        }
    }
}
