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
    public class ExamResultConfiguration : IEntityTypeConfiguration<ExamResult>
    {
        public void Configure(EntityTypeBuilder<ExamResult> builder)
        {
            builder.ToTable("ExamResults");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Score).IsRequired(true);

            builder.HasOne(t => t.Category).WithMany(pc => pc.ExamResults)
           .HasForeignKey(pc => pc.CategoryId);

            builder.HasOne(x => x.AppUser).WithMany(x => x.ExamResults)
           .HasForeignKey(x => x.UserId);
        }
    }
}
