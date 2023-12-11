using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class UpdataModelBuilder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "5dacf20d-3aea-4826-aacf-bfc13ba85e3e");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "b324ee1c-970e-4ab7-b48e-150b0391c765");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "24a6f462-3bb3-4c8f-9056-aa9f71a54066", "AQAAAAEAACcQAAAAEG2VPbQ5OltG36FTSvQqdFQyMJRwX2VNkzQy2BiDRsETButdraQ9hI7RtKQtKBA1FA==" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Status" },
                values: new object[] { 1, "Retest", true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "6eb44768-efe7-4e85-9cdb-162f42d563bf");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "fe2254d1-d25b-429e-9fb4-5a371cf4d89d");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4cfb7c76-f3d7-417c-9cc3-c948edb951c0", "AQAAAAEAACcQAAAAEIsmdH6liJf9ea1BbuAm6ILQCZGvKSm7t7oTbFqQGBnz42+ygc7R1fa470Bk6c8/yQ==" });
        }
    }
}
