using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddChooseNumberLoiTaiCongDoan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChooseNumber",
                table: "TTTTLoiTaiCongDoan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "bcc1ab28-776f-45aa-aa97-6b270a44f18f");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "2d8f4d84-84c3-4301-9475-04476f3092c5");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6a03c5ed-c09d-4c5b-861f-dd978d512abf", "AQAAAAEAACcQAAAAEFJuqFL1gqsUqe1ZWy4nt18bme1mYkSlRLXe5o6PdxsxrfUlJqrxYbLLjaysvXLo+A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChooseNumber",
                table: "TTTTLoiTaiCongDoan");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "30446434-f6b8-4c4f-83d0-dab46b05f460");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "5dd9d449-d170-4495-9b26-587cfbfc0a94");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c20b5b31-af48-4f09-bbf9-f035e55b32df", "AQAAAAEAACcQAAAAEC7OIuWXh1KJsatyYKu5F3fQD6s6fha7/sk7XpH4XMCAQC9UN/QA4yCGrMKL3Wo25A==" });
        }
    }
}
