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
    public class ReadResultConfiguration : IEntityTypeConfiguration<ReadResult>
    {
        public void Configure(EntityTypeBuilder<ReadResult> builder)
        {
            builder.ToTable("ReadResult");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();     
            
            builder.Property(x => x.ReadPostId).IsRequired(true);
            builder.HasOne(t => t.ReadPost).WithMany(pc => pc.readResults)
                .HasForeignKey(pc => pc.ReadPostId);

            builder.Property(x => x.UserId).IsRequired(true);
            builder.HasOne(t => t.AppUser).WithMany(pc => pc.readResults)
                .HasForeignKey(pc => pc.UserId);
        }
    }
}
