using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Logexamtrinhtuthaotac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "FinalScore",
                table: "LogExams",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Dapandung",
                table: "LogExams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Cautraloi",
                table: "LogExams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "LoaiCauHoi",
                table: "LogExams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "LogExams",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "LogExamTrinhtuthaotacs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThuTu = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<int>(type: "int", nullable: false),
                    LogExamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogExamTrinhtuthaotacs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogExamTrinhtuthaotacs_LogExams_LogExamId",
                        column: x => x.LogExamId,
                        principalTable: "LogExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "d6da5d98-dcd7-43e2-ab0d-7a71f2c9c886");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "df55a115-8354-4fd0-be8b-6787d66d73ea");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "600421d2-6329-4480-b2ec-82464d51d3ef", "AQAAAAEAACcQAAAAEMbyed2sckz4sBmQACvvqzIxtvea8xAbyr3SuLhV6HwqfVQtHjWaJBLOgDqR4HzJTA==" });

            migrationBuilder.CreateIndex(
                name: "IX_LogExamTrinhtuthaotacs_LogExamId",
                table: "LogExamTrinhtuthaotacs",
                column: "LogExamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogExamTrinhtuthaotacs");

            migrationBuilder.DropColumn(
                name: "LoaiCauHoi",
                table: "LogExams");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "LogExams");

            migrationBuilder.AlterColumn<int>(
                name: "FinalScore",
                table: "LogExams",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "Dapandung",
                table: "LogExams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cautraloi",
                table: "LogExams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "c0c5c7ed-3eb1-4f00-bb5b-b802b3897106");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "5eaaf461-ee0a-4143-bcae-4f7694edbdb9");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8c409fc5-0eca-471b-a66e-2f61faafd655", "AQAAAAEAACcQAAAAELlnfWoa4qrIs3jC5G4oietEqL8Pa2J8qRI5uRR5TBpEDhsILZYuHM8pj+z0xhnQ5w==" });
        }
    }
}
