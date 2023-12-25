using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddFinalScoreScoretoLogexamTTTT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "FinalScore",
                table: "LogExamTrinhtuthaotacs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalScore",
                table: "LogExamTrinhtuthaotacs");

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
    }
}
