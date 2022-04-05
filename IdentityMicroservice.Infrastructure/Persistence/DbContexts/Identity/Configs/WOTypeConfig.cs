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
    public class WOTypeConfig : IEntityTypeConfiguration<WOType>
    {
        public void Configure(EntityTypeBuilder<WOType> builder)
        {
            builder.ToTable(nameof(WOType));
            builder.HasKey(prop => prop.Id);


            builder.Property(prop => prop.ChangedByUserId).IsRequired().HasMaxLength(50);
            builder.Property(prop => prop.WOTypeCode).IsRequired().HasMaxLength(100);
            builder.Property(prop => prop.WOTypeName).IsRequired().HasMaxLength(100);
            builder.Property(prop => prop.DurationMinutes).IsRequired().HasPrecision(18,2);

            //builder.HasOne(u => u.IdentityUser).WithMany(wot => wot.WOTypes); // hasforeignkey? ->ChangedByUserId==userid 
            builder.HasMany(wots => wots.WOTypeScenarios).WithOne(wot => wot.WOType).HasForeignKey(wot => wot.WOTypeId);
        }
    }
}
