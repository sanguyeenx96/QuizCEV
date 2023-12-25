﻿// <auto-generated />
using System;
using Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(TracNghiemCEVDbContext))]
    partial class TracNghiemCEVDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Data.Entities.AppRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                            ConcurrencyStamp = "cc783012-c35f-4d54-bae7-b6a661c7506b",
                            Description = "Administrator role",
                            Name = "admin",
                            NormalizedName = "admin"
                        },
                        new
                        {
                            Id = new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                            ConcurrencyStamp = "f90feacc-f38a-4c46-8a17-162e32ac6abb",
                            Description = "User role",
                            Name = "user",
                            NormalizedName = "user"
                        });
                });

            modelBuilder.Entity("Data.Entities.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("CellId")
                        .HasMaxLength(200)
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CellId");

                    b.ToTable("AppUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                            AccessFailedCount = 0,
                            CellId = 1,
                            ConcurrencyStamp = "d7bcd088-48c5-4f7c-9b42-9c2095d173c9",
                            Email = "smt.ngocsang@gmail.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "Nguyen Ngoc Sang",
                            NormalizedEmail = "smt.ngocsang@gmail.com",
                            NormalizedUserName = "admin",
                            PasswordHash = "AQAAAAEAACcQAAAAEGuFWKtZDXNkbGR+QGBUXYjeuMzRwtAccRIbxbqTLQVhF6SewC8uxXMptwFKUiDc5Q==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("Data.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("Data.Entities.CauHoiTrinhTuThaoTac", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CauHoiTuLuanId")
                        .HasColumnType("int");

                    b.Property<float?>("Score")
                        .HasColumnType("real");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThuTu")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CauHoiTuLuanId");

                    b.ToTable("CauHoiTrinhTuThaoTac", (string)null);
                });

            modelBuilder.Entity("Data.Entities.CauHoiTuLuan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<float?>("Score")
                        .HasColumnType("real");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("CauHoiTuLuan", (string)null);
                });

            modelBuilder.Entity("Data.Entities.Cell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("Cells", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ModelId = 1,
                            Name = "None"
                        });
                });

            modelBuilder.Entity("Data.Entities.Dept", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Depts", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "MFE"
                        });
                });

            modelBuilder.Entity("Data.Entities.ExamResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<float>("Score")
                        .HasColumnType("real");

                    b.Property<int>("ThoiGianChoPhepLamBai")
                        .HasColumnType("int");

                    b.Property<int>("ThoiGianLamBai")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("ExamResults", (string)null);
                });

            modelBuilder.Entity("Data.Entities.LogExam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Cauhoi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cautraloi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dapandung")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExamResultId")
                        .HasColumnType("int");

                    b.Property<float>("FinalScore")
                        .HasColumnType("real");

                    b.Property<string>("LoaiCauHoi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Score")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ExamResultId");

                    b.ToTable("LogExams", (string)null);
                });

            modelBuilder.Entity("Data.Entities.LogExamDiemChuY", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Answer")
                        .HasColumnType("int");

                    b.Property<int>("CorrectAnswer")
                        .HasColumnType("int");

                    b.Property<int>("LogExamId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LogExamId");

                    b.ToTable("LogExamDiemChuY", (string)null);
                });

            modelBuilder.Entity("Data.Entities.LogExamDoiSach", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LogExamId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LogExamId");

                    b.ToTable("LogExamDoiSach", (string)null);
                });

            modelBuilder.Entity("Data.Entities.LogExamLoiTaiCongDoan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Answer")
                        .HasColumnType("int");

                    b.Property<int>("CorrectAnswer")
                        .HasColumnType("int");

                    b.Property<int>("LogExamId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LogExamId");

                    b.ToTable("LogExamLoiTaiCongDoan", (string)null);
                });

            modelBuilder.Entity("Data.Entities.LogExamTrinhtuthaotac", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Answer")
                        .HasColumnType("int");

                    b.Property<float>("FinalScore")
                        .HasColumnType("real");

                    b.Property<int>("LogExamId")
                        .HasColumnType("int");

                    b.Property<float>("Score")
                        .HasColumnType("real");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThuTu")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LogExamId");

                    b.ToTable("LogExamTrinhtuthaotacs", (string)null);
                });

            modelBuilder.Entity("Data.Entities.LoiTaiCongDoanDoiSach", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("LoiTaiCongDoanId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LoiTaiCongDoanId");

                    b.ToTable("LoiTaiCongDoanDoiSach", (string)null);
                });

            modelBuilder.Entity("Data.Entities.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DeptId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DeptId");

                    b.ToTable("Models", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DeptId = 1,
                            Name = "MFE 1"
                        });
                });

            modelBuilder.Entity("Data.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("QA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QCorrectAns")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Score")
                        .HasColumnType("real");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Questions", (string)null);
                });

            modelBuilder.Entity("Data.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Settings", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Retest",
                            Status = true
                        });
                });

            modelBuilder.Entity("Data.Entities.TTTTDiemChuY", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<Guid>("CauhoitrinhtuthaotacId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CauhoitrinhtuthaotacId");

                    b.ToTable("TTTTDiemChuY", (string)null);
                });

            modelBuilder.Entity("Data.Entities.TTTTLoiTaiCongDoan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<Guid>("CauhoitrinhtuthaotacId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CauhoitrinhtuthaotacId");

                    b.ToTable("TTTTLoiTaiCongDoan", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AppRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AppUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("AppUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("AppUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                            RoleId = new Guid("8d04dce2-969a-435d-bba4-df3f325983dc")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("AppUserTokens", (string)null);
                });

            modelBuilder.Entity("Data.Entities.AppUser", b =>
                {
                    b.HasOne("Data.Entities.Cell", "Cell")
                        .WithMany("AppUsers")
                        .HasForeignKey("CellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cell");
                });

            modelBuilder.Entity("Data.Entities.CauHoiTrinhTuThaoTac", b =>
                {
                    b.HasOne("Data.Entities.CauHoiTuLuan", "CauHoiTuLuan")
                        .WithMany("cauHoiTrinhTuThaoTacs")
                        .HasForeignKey("CauHoiTuLuanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CauHoiTuLuan");
                });

            modelBuilder.Entity("Data.Entities.CauHoiTuLuan", b =>
                {
                    b.HasOne("Data.Entities.Category", "Category")
                        .WithMany("cauHoiTuLuans")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Data.Entities.Cell", b =>
                {
                    b.HasOne("Data.Entities.Model", "Model")
                        .WithMany("Cells")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });

            modelBuilder.Entity("Data.Entities.ExamResult", b =>
                {
                    b.HasOne("Data.Entities.Category", "Category")
                        .WithMany("ExamResults")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.AppUser", "AppUser")
                        .WithMany("ExamResults")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Data.Entities.LogExam", b =>
                {
                    b.HasOne("Data.Entities.ExamResult", "ExamResult")
                        .WithMany("LogExams")
                        .HasForeignKey("ExamResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExamResult");
                });

            modelBuilder.Entity("Data.Entities.LogExamDiemChuY", b =>
                {
                    b.HasOne("Data.Entities.LogExam", "LogExam")
                        .WithMany("logExamDiemChuYs")
                        .HasForeignKey("LogExamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LogExam");
                });

            modelBuilder.Entity("Data.Entities.LogExamDoiSach", b =>
                {
                    b.HasOne("Data.Entities.LogExam", "LogExam")
                        .WithMany("logExamDoiSaches")
                        .HasForeignKey("LogExamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LogExam");
                });

            modelBuilder.Entity("Data.Entities.LogExamLoiTaiCongDoan", b =>
                {
                    b.HasOne("Data.Entities.LogExam", "LogExam")
                        .WithMany("logExamLoiTaiCongDoans")
                        .HasForeignKey("LogExamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LogExam");
                });

            modelBuilder.Entity("Data.Entities.LogExamTrinhtuthaotac", b =>
                {
                    b.HasOne("Data.Entities.LogExam", "LogExam")
                        .WithMany("LogExamTrinhtuthaotacs")
                        .HasForeignKey("LogExamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LogExam");
                });

            modelBuilder.Entity("Data.Entities.LoiTaiCongDoanDoiSach", b =>
                {
                    b.HasOne("Data.Entities.TTTTLoiTaiCongDoan", "TTTTLoiTaiCongDoans")
                        .WithMany("LoiTaiCongDoanDoiSachs")
                        .HasForeignKey("LoiTaiCongDoanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TTTTLoiTaiCongDoans");
                });

            modelBuilder.Entity("Data.Entities.Model", b =>
                {
                    b.HasOne("Data.Entities.Dept", "Dept")
                        .WithMany("Models")
                        .HasForeignKey("DeptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dept");
                });

            modelBuilder.Entity("Data.Entities.Question", b =>
                {
                    b.HasOne("Data.Entities.Category", "Category")
                        .WithMany("Questions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Data.Entities.TTTTDiemChuY", b =>
                {
                    b.HasOne("Data.Entities.CauHoiTrinhTuThaoTac", "CauHoiTrinhTuThaoTac")
                        .WithMany("TTTTDiemChuYs")
                        .HasForeignKey("CauhoitrinhtuthaotacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CauHoiTrinhTuThaoTac");
                });

            modelBuilder.Entity("Data.Entities.TTTTLoiTaiCongDoan", b =>
                {
                    b.HasOne("Data.Entities.CauHoiTrinhTuThaoTac", "CauHoiTrinhTuThaoTac")
                        .WithMany("TTTTLoiTaiCongDoans")
                        .HasForeignKey("CauhoitrinhtuthaotacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CauHoiTrinhTuThaoTac");
                });

            modelBuilder.Entity("Data.Entities.AppUser", b =>
                {
                    b.Navigation("ExamResults");
                });

            modelBuilder.Entity("Data.Entities.Category", b =>
                {
                    b.Navigation("ExamResults");

                    b.Navigation("Questions");

                    b.Navigation("cauHoiTuLuans");
                });

            modelBuilder.Entity("Data.Entities.CauHoiTrinhTuThaoTac", b =>
                {
                    b.Navigation("TTTTDiemChuYs");

                    b.Navigation("TTTTLoiTaiCongDoans");
                });

            modelBuilder.Entity("Data.Entities.CauHoiTuLuan", b =>
                {
                    b.Navigation("cauHoiTrinhTuThaoTacs");
                });

            modelBuilder.Entity("Data.Entities.Cell", b =>
                {
                    b.Navigation("AppUsers");
                });

            modelBuilder.Entity("Data.Entities.Dept", b =>
                {
                    b.Navigation("Models");
                });

            modelBuilder.Entity("Data.Entities.ExamResult", b =>
                {
                    b.Navigation("LogExams");
                });

            modelBuilder.Entity("Data.Entities.LogExam", b =>
                {
                    b.Navigation("LogExamTrinhtuthaotacs");

                    b.Navigation("logExamDiemChuYs");

                    b.Navigation("logExamDoiSaches");

                    b.Navigation("logExamLoiTaiCongDoans");
                });

            modelBuilder.Entity("Data.Entities.Model", b =>
                {
                    b.Navigation("Cells");
                });

            modelBuilder.Entity("Data.Entities.TTTTLoiTaiCongDoan", b =>
                {
                    b.Navigation("LoiTaiCongDoanDoiSachs");
                });
#pragma warning restore 612, 618
        }
    }
}
