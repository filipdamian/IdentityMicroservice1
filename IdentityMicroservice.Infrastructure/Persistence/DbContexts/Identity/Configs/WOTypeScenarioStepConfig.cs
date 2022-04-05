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
    public class WOTypeScenarioStepConfig : IEntityTypeConfiguration<WOTypeScenarioStep>
    {
        public void Configure(EntityTypeBuilder<WOTypeScenarioStep> builder)
        {
            builder.ToTable(nameof(WOTypeScenarioStep));
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.StepName).IsRequired().HasMaxLength(100);
            builder.Property(prop => prop.ScreenCode).IsRequired().HasMaxLength(50);

            builder.HasOne(wots => wots.WOTypeScenario).WithMany(wotss => wotss.WOTypeScenarioSteps).HasForeignKey(wotss => wotss.WOScenarioId);
            builder.HasOne(sc => sc.Screen).WithOne(wotss => wotss.WOTypeScenarioStep);
        }
    }
}
