using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Username).IsRequired(true);

            builder.Property(x => x.Password).IsRequired(true);

            builder.Property(x => x.Role).IsRequired(true);

            builder.Property(x => x.Role).HasDefaultValue(Role.User);

            builder.Property(x => x.MaNV).IsRequired(true);

            builder.Property(x => x.Hoten).IsRequired(true);

            builder.Property(x => x.Bophan).IsRequired(true);
        }
    }
}
