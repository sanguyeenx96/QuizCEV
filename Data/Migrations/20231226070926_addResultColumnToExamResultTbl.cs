using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class addResultColumnToExamResultTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Result",
                table: "ExamResults",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "bcf5b0a6-7ea7-4d3b-b0b9-b2af0664eb09");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "b04f72f5-a347-4789-9948-9305902c233d");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e29c3f4d-dd6b-49ff-9d4e-e3e099ab1cf8", "AQAAAAEAACcQAAAAEBeGMzo213GT9/Wq7PN5669U55w8QYYSI/J8itm8hnFajF8GDi05df8MMeI2B+QaJg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "ExamResults");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "f90feacc-f38a-4c46-8a17-162e32ac6abb");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "cc783012-c35f-4d54-bae7-b6a661c7506b");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d7bcd088-48c5-4f7c-9b42-9c2095d173c9", "AQAAAAEAACcQAAAAEGuFWKtZDXNkbGR+QGBUXYjeuMzRwtAccRIbxbqTLQVhF6SewC8uxXMptwFKUiDc5Q==" });
        }
    }
}
