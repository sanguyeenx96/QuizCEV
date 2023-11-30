using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    public class CauHoiTrinhTuThaoTacConfiguration : IEntityTypeConfiguration<CauHoiTrinhTuThaoTac>
    {
        public void Configure(EntityTypeBuilder<CauHoiTrinhTuThaoTac> builder)
        {
            builder.ToTable("CauHoiTrinhTuThaoTac");

            builder.HasKey(x => x.Id);

            builder.HasOne(t => t.CauHoiTuLuan).WithMany(pc => pc.cauHoiTrinhTuThaoTacs)
                .HasForeignKey(pc => pc.CauHoiTuLuanId);
        }
    }
}
