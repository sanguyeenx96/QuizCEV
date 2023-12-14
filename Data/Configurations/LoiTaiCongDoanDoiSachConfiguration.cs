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
    public class LoiTaiCongDoanDoiSachConfiguration : IEntityTypeConfiguration<LoiTaiCongDoanDoiSach>
    {
        public void Configure(EntityTypeBuilder<LoiTaiCongDoanDoiSach> builder)
        {
            builder.ToTable("LoiTaiCongDoanDoiSach");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(t => t.TTTTLoiTaiCongDoans).WithMany(pc => pc.LoiTaiCongDoanDoiSachs)
            .HasForeignKey(pc => pc.LoiTaiCongDoanId);
        }
    }
}
