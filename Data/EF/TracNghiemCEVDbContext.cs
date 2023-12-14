using Data.Configurations;
using Data.Entities;
using Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    public class TracNghiemCEVDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public TracNghiemCEVDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new CategoryConfigration());
            modelBuilder.ApplyConfiguration(new ExamResultConfiguration());
            modelBuilder.ApplyConfiguration(new LogExamConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new CauHoiTuLuanConfiguration());
            modelBuilder.ApplyConfiguration(new CauHoiTrinhTuThaoTacConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new DeptConfiguration());
            modelBuilder.ApplyConfiguration(new LogExamTrinhtuthaotacConfiguration());
            modelBuilder.ApplyConfiguration(new ModelConfiguration());
            modelBuilder.ApplyConfiguration(new CellConfiguration());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());

            modelBuilder.ApplyConfiguration(new TTTTDiemChuYConfiguration());
            modelBuilder.ApplyConfiguration(new TTTTLoiTaiCongDoanConfiguration());
            modelBuilder.ApplyConfiguration(new LoiTaiCongDoanDoiSachConfiguration());

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
                .HasKey(x => new
                {
                    x.UserId,
                    x.RoleId
                });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins")
                .HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
                .HasKey(x => x.UserId);

            //Data seeding
            modelBuilder.Seed();
            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<LogExam> LogExams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<CauHoiTuLuan> CauHoiTuLuans { get; set; }
        public DbSet<CauHoiTrinhTuThaoTac> cauHoiTrinhTuThaoTacs { get; set; }
        public DbSet<Dept> Depts { get; set; }
        public DbSet<LogExamTrinhtuthaotac> logExamTrinhtuthaotacs { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<TTTTDiemChuY> DiemChuYs { get; set; }
        public DbSet<TTTTLoiTaiCongDoan> LoiTaiCongDoans { get; set; }
        public DbSet<LoiTaiCongDoanDoiSach> LoiTaiCongDoanDoiSaches { get; set; }
    }
}
