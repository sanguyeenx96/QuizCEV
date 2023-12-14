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
    public class TTTTDiemChuYConfiguration : IEntityTypeConfiguration<TTTTDiemChuY>
    {
        public void Configure(EntityTypeBuilder<TTTTDiemChuY> builder)
        {
            builder.ToTable("TTTTDiemChuY");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(t => t.CauHoiTrinhTuThaoTac).WithMany(pc => pc.TTTTDiemChuYs)
            .HasForeignKey(pc => pc.CauhoitrinhtuthaotacId);
        }
    }
}
