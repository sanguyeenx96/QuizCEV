using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddReadsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ReadCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadPost_ReadCategory_ReadCategoryId",
                        column: x => x.ReadCategoryId,
                        principalTable: "ReadCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadPostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadResult_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadResult_ReadPost_ReadPostId",
                        column: x => x.ReadPostId,
                        principalTable: "ReadPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "2f83b0b9-7248-4f10-93a7-d5ba10564aea");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "833ec544-6c35-434f-b616-9e7653911fc9");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "87e302cc-0536-4943-a470-ea8305df86c6", "AQAAAAEAACcQAAAAEGkkCfTEPF3DC3JNYJtw9dVt89m368ktfO/cMEMXFTJhNy+XMOhrKyzSyVkx37kOMA==" });

            migrationBuilder.CreateIndex(
                name: "IX_ReadPost_ReadCategoryId",
                table: "ReadPost",
                column: "ReadCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadResult_ReadPostId",
                table: "ReadResult",
                column: "ReadPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadResult_UserId",
                table: "ReadResult",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadResult");

            migrationBuilder.DropTable(
                name: "ReadPost");

            migrationBuilder.DropTable(
                name: "ReadCategory");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "5219feb8-757b-4db4-946a-dcc104d4591e");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "6d537fbf-bb5d-4491-8d86-0a345766335a");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "add7c7a0-3979-49d4-848b-619648ee6b58", "AQAAAAEAACcQAAAAEEpzjA/Gy8YUKZrkv6w+PjjJralFlgHROBZKluwXgTetGTnaZyN2jHQlugg0uScg4A==" });
        }
    }
}
