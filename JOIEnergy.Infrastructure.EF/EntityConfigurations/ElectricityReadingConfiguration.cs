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
    public class ElectricityReadingConfiguration : IEntityTypeConfiguration<ElectricityReading>
    {
        public void Configure(EntityTypeBuilder<ElectricityReading> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Reading).HasPrecision(18, 4);
        }
    }
}
