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
    public class WOTypeScenarioConfig : IEntityTypeConfiguration<WOTypeScenario>
    {
        public void Configure(EntityTypeBuilder<WOTypeScenario> builder)
        {
            builder.ToTable(nameof(WOTypeScenario));
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.ScenarioCode).IsRequired().HasMaxLength(100);
            builder.Property(prop => prop.ScenarioName).IsRequired().HasMaxLength(50);

            builder.HasOne(wots => wots.WOType).WithMany(wot => wot.WOTypeScenarios).HasForeignKey(wot => wot.WOTypeId);
            builder.HasMany(wotss => wotss.WOTypeScenarioSteps).WithOne(wots => wots.WOTypeScenario);
        }
    }
}
