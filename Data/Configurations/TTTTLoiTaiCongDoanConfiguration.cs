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
    public class TTTTLoiTaiCongDoanConfiguration : IEntityTypeConfiguration<TTTTLoiTaiCongDoan>
    {
        public void Configure(EntityTypeBuilder<TTTTLoiTaiCongDoan> builder)
        {
            builder.ToTable("TTTTLoiTaiCongDoan");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(t => t.CauHoiTrinhTuThaoTac).WithMany(pc => pc.TTTTLoiTaiCongDoans)
            .HasForeignKey(pc => pc.CauhoitrinhtuthaotacId);
        }
    }
}
