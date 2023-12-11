using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dept>().HasData(new Dept
            {
                Id = 1,                
                Name = "MFE"
            });

            modelBuilder.Entity<Model>().HasData(new Model
            {
                Id = 1,
                Name = "MFE 1",
                DeptId =1
            });

            modelBuilder.Entity<Cell>().HasData(new Cell
            {
                Id = 1,
                Name = "None",
                ModelId = 1
            });

            // any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "smt.ngocsang@gmail.com",
                NormalizedEmail = "smt.ngocsang@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Abcd1234$"),
                SecurityStamp = string.Empty,
                Name = "Nguyen Ngoc Sang",
                CellId = 1
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });


            // Thêm user role
            var userRoleId = new Guid("470F4021-29D8-4C8E-A9DE-527571683D86");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = userRoleId,
                Name = "user",
                NormalizedName = "user",
                Description = "User role"
            });

            //Thêm Setting
            modelBuilder.Entity<Setting>().HasData(new Setting
            {
                Id = 1,
                Name = "Retest",
                Status = true
            });

        }
    }
}