using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddScoreToCauHoiTrinhTuThaoTac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "CauHoiTrinhTuThaoTac",
                type: "real",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "CauHoiTrinhTuThaoTac");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "f5c39777-6e98-480f-9276-bb315377e819");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "03f42a7e-88d7-4c6a-991a-650a83bbe74d");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "da80ffdc-12de-4223-b8a3-4c17cc0c76d6", "AQAAAAEAACcQAAAAEDVDlashnVFOKW03INKRI5eRh8b4dIwK3PD9a1LaRI5XG/ysNLc1E4lPd9dmtXEhmA==" });
        }
    }
}
