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
    public class LogExamLoiTaiCongDoanConfiguration : IEntityTypeConfiguration<LogExamLoiTaiCongDoan>
    {
        public void Configure(EntityTypeBuilder<LogExamLoiTaiCongDoan> builder)
        {
            builder.ToTable("LogExamLoiTaiCongDoan");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(t => t.LogExam).WithMany(pc => pc.logExamLoiTaiCongDoans)
            .HasForeignKey(pc => pc.LogExamId);
        }
    }
}
