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
    public class ReadCategoryConfiguration : IEntityTypeConfiguration<ReadCategory>
    {
        public void Configure(EntityTypeBuilder<ReadCategory> builder)
        {
            builder.ToTable("ReadCategory");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Title).IsRequired(true);
        }
    }
}
