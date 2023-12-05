using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class ThemCotThoiGian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThoiGianChoPhepLamBai",
                table: "ExamResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThoiGianLamBai",
                table: "ExamResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "893ba1aa-0655-42b7-a3ff-76aa5a4afc86");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "9c7d7828-5b56-480a-b51d-23cb5bbc9544");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5fb73bcf-9248-4c5e-8ba2-e5279f24cd94", "AQAAAAEAACcQAAAAEJYJybz5+zMwsn19SAdIDh9mOEIW5YTme20M1W8jBDjwSTjn3bpN1HP/Q6aHbXlxkA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThoiGianChoPhepLamBai",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "ThoiGianLamBai",
                table: "ExamResults");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "38c52804-e595-4d71-876f-847461c986e6");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "fb0fb2e4-b39b-4851-8f80-51e67acff30a");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cefd7a1e-7736-40fc-9b0f-d7fa53b5e436", "AQAAAAEAACcQAAAAEA+kCYdrt9HcuJpfNTp2791DeYxsPY8Yrl6wmkvju6byd3D+FcqFyiWfNpToy5310w==" });
        }
    }
}
