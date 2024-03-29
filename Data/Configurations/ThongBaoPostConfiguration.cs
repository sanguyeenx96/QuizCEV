﻿using Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    public class ThongBaoPostConfiguration : IEntityTypeConfiguration<ThongBaoPost>
    {
        public void Configure(EntityTypeBuilder<ThongBaoPost> builder)
        {
            builder.ToTable("ThongBaoPost");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).IsRequired(true);
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.FilePath).IsRequired(false);
            builder.Property(x => x.Content).IsRequired(true);
            builder.Property(x => x.ThongBaoCategoryId).IsRequired(true);
            builder.HasOne(t => t.thongBaoCategory).WithMany(pc => pc.thongBaoPosts)
                .HasForeignKey(pc => pc.ThongBaoCategoryId);
        }
    }
}
