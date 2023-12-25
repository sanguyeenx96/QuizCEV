using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddScoretoLogexamTTTT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "LogExamTrinhtuthaotacs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "c7de6ae5-ee13-4482-90a8-685d017505da");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "7dc1f8de-5065-4aef-b027-e75901890eb7");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b295ea59-e4af-43d8-b2f7-3fd70e1e5eb2", "AQAAAAEAACcQAAAAECEKuNVfTrqiEm4crJjF/DFttnMMBwGl4XOVoZlX9uR2ZrLu9pw6LqjmF784T1E25Q==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "LogExamTrinhtuthaotacs");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "cf02fee4-45ef-478e-9da7-f6b1fac21360");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "962b93fd-f930-41c5-bc3a-c5e148c0dc14");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f99f57bc-19e6-487a-b998-e7ba5534c74a", "AQAAAAEAACcQAAAAECYwHxKXyRRW7+O/dFO3mOE8w95UfhH19xeCIqF2Hl8P3VkdCBXRI+emwdvtbkJapw==" });
        }
    }
}
