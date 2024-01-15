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
    public class PostPostConfiguration : IEntityTypeConfiguration<PostPost>
    {
        public void Configure(EntityTypeBuilder<PostPost> builder)
        {
            builder.ToTable("PostPost");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).IsRequired(true);
            builder.Property(x => x.Description).IsRequired(true);
            builder.Property(x => x.Content).IsRequired(true);
            builder.Property(x => x.PostCategoryId).IsRequired(true);
            builder.HasOne(t => t.postCategory).WithMany(pc => pc.PostPosts)
                .HasForeignKey(pc => pc.PostCategoryId);
        }
    }
}
