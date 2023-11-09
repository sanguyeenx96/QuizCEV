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
    public class LogExamConfiguration : IEntityTypeConfiguration<LogExam>
    {
        public void Configure(EntityTypeBuilder<LogExam> builder)
        {
            builder.ToTable("LogExams");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.ExamResultId).IsRequired(true);

            builder.Property(x => x.Cauhoi).IsRequired(true);

            builder.HasOne(t => t.ExamResult).WithMany(pc => pc.LogExams)
            .HasForeignKey(pc => pc.ExamResultId);
        }
    }
}
