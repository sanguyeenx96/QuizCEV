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
    public class ThongBaoCategoryConfiguration : IEntityTypeConfiguration<ThongBaoCategory>
    {
        public void Configure(EntityTypeBuilder<ThongBaoCategory> builder)
        {
            builder.ToTable("ThongBaoCategory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).IsRequired(true);
        }
    }
}
