using Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Enums;

namespace Data.Configurations
{
    public class ReadPostConfiguration : IEntityTypeConfiguration<ReadPost>
    {
        public void Configure(EntityTypeBuilder<ReadPost> builder)
        {
            builder.ToTable("ReadPost");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).IsRequired(true);
            builder.Property(x => x.Description).IsRequired(true);
            builder.Property(x => x.ThumbImage).IsRequired(false);
            builder.Property(x => x.Content).IsRequired(true);
            builder.Property(x => x.Status).IsRequired(true);
            builder.Property(x => x.Status).HasDefaultValue(Status.Active);

            builder.Property(x => x.ReadCategoryId).IsRequired(true);
            builder.HasOne(t => t.readCategory).WithMany(pc => pc.readPosts)
                .HasForeignKey(pc => pc.ReadCategoryId);
        }
    }
}
