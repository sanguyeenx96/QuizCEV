using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class addThongBaoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postPosts_postCategories_PostCategoryId",
                table: "postPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_postPosts",
                table: "postPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_postCategories",
                table: "postCategories");

            migrationBuilder.RenameTable(
                name: "postPosts",
                newName: "PostPost");

            migrationBuilder.RenameTable(
                name: "postCategories",
                newName: "PostCategory");

            migrationBuilder.RenameIndex(
                name: "IX_postPosts_PostCategoryId",
                table: "PostPost",
                newName: "IX_PostPost_PostCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostPost",
                table: "PostPost",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCategory",
                table: "PostCategory",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ThongBaoCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoPost",
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
                    ThongBaoCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongBaoPost_ThongBaoCategory_ThongBaoCategoryId",
                        column: x => x.ThongBaoCategoryId,
                        principalTable: "ThongBaoCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "6fa58180-6a2c-4a76-af6c-b229b168fc8e");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "cbf6119e-ac5c-4c92-a862-c172da8a15f3");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "079c9f20-ab3d-4279-8a7d-31a3bd71f7af", "AQAAAAEAACcQAAAAECygAp/UzqMoPQGQz4IIm00K4OX8MdhlylyxfW0WexoKHIoWtXA/yhwOL4+cPpPs+g==" });

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoPost_ThongBaoCategoryId",
                table: "ThongBaoPost",
                column: "ThongBaoCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostPost_PostCategory_PostCategoryId",
                table: "PostPost",
                column: "PostCategoryId",
                principalTable: "PostCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostPost_PostCategory_PostCategoryId",
                table: "PostPost");

            migrationBuilder.DropTable(
                name: "ThongBaoPost");

            migrationBuilder.DropTable(
                name: "ThongBaoCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostPost",
                table: "PostPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCategory",
                table: "PostCategory");

            migrationBuilder.RenameTable(
                name: "PostPost",
                newName: "postPosts");

            migrationBuilder.RenameTable(
                name: "PostCategory",
                newName: "postCategories");

            migrationBuilder.RenameIndex(
                name: "IX_PostPost_PostCategoryId",
                table: "postPosts",
                newName: "IX_postPosts_PostCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_postPosts",
                table: "postPosts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_postCategories",
                table: "postCategories",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "9b50e80b-1043-45d8-abbd-fad4ba8b1e50");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "1c2805fa-756c-4b22-ad41-ac739a310c4f");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5ffecd8a-6bb3-4a93-8ade-f1843e021393", "AQAAAAEAACcQAAAAEG58eD6nC3lWYVMXPxK1NTTaNjU8F9kVr0BB+RGCMUOD+KHgerIKP7WEo62uUXdF/Q==" });

            migrationBuilder.AddForeignKey(
                name: "FK_postPosts_postCategories_PostCategoryId",
                table: "postPosts",
                column: "PostCategoryId",
                principalTable: "postCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
