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
    public class ScreenConfig : IEntityTypeConfiguration<Screen>
    {
        public void Configure(EntityTypeBuilder<Screen> builder)
        {
            builder.ToTable(nameof(Screen));
            builder.HasKey(prop => prop.Id);


            builder.Property(prop => prop.ScreenCode).IsRequired().HasMaxLength(100);
            builder.Property(prop => prop.ScreenName).IsRequired().HasMaxLength(50);

            builder.HasOne(wotss => wotss.WOTypeScenarioStep).WithOne(sc => sc.Screen);
        }
    }
}
