using Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    public class CauHoiTuLuanConfiguration : IEntityTypeConfiguration<CauHoiTuLuan>
    {
        public void Configure(EntityTypeBuilder<CauHoiTuLuan> builder)
        {
            builder.ToTable("CauHoiTuLuan");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(t => t.Category).WithMany(pc => pc.cauHoiTuLuans)
            .HasForeignKey(pc => pc.CategoryId);
        }
    }
}
