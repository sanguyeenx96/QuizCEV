using Data.Configurations;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    public class TracNghiemCEVDbContext : DbContext
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

            modelBuilder.ApplyConfiguration(new UserConfiguration());

            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<LogExam> LogExams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<CauHoiTuLuan> CauHoiTuLuans { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
