using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class addUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "64f47897-6ef3-4f80-9ac1-94ae38dd195a");

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("470f4021-29d8-4c8e-a9de-527571683d86"), "c90ef37f-66c8-41d3-82c9-9fba22555216", "User role", "user", "user" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4584d247-af6e-43a3-8748-e6c40b1a1517", "AQAAAAEAACcQAAAAEFgZjD19D1AepX9+wjPs1j7RHRVumTeaBWg40vp/XXi4zMAc4Sd4Dcc9i4S3abURYw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "f88e6f7e-c70a-44fa-a72e-367b247ac5de");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4f21d2e2-38cd-432a-a68d-2d6197a11639", "AQAAAAEAACcQAAAAEDe1gFS1cHH4cF3vhfbLNkpP4kAJ3e35pnWw7QHRf3EyRxixXYvFUJDZMoLiRTWFZA==" });
        }
    }
}
