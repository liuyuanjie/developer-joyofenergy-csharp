using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JOIEnergy.Infrastructure.EF.EntityConfigurations
{
    public class SmartMeterConfiguration : IEntityTypeConfiguration<SmartMeter>
    {
        public void Configure(EntityTypeBuilder<SmartMeter> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasIndex(x => x.SmartMeterId).IsUnique();
            builder.HasAlternateKey(x => x.SmartMeterId);
            builder.HasMany(b => b.ElectricityReadings)
                .WithOne()
                .HasForeignKey(nameof(ElectricityReading.SmartMeterId))
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
