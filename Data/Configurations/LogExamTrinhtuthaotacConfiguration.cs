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
    public class LogExamTrinhtuthaotacConfiguration : IEntityTypeConfiguration<LogExamTrinhtuthaotac>
    {
        public void Configure(EntityTypeBuilder<LogExamTrinhtuthaotac> builder)
        {
            builder.ToTable("LogExamTrinhtuthaotacs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(t => t.LogExam).WithMany(pc => pc.LogExamTrinhtuthaotacs)
            .HasForeignKey(pc => pc.LogExamId);
        }
    }
}
