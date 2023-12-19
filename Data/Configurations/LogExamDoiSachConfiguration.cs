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
    public class LogExamDoiSachConfiguration : IEntityTypeConfiguration<LogExamDoiSach>
    {
        public void Configure(EntityTypeBuilder<LogExamDoiSach> builder)
        {
            builder.ToTable("LogExamDoiSach");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(t => t.LogExam).WithMany(pc => pc.logExamDoiSaches)
            .HasForeignKey(pc => pc.LogExamId);
        }
    }
}
