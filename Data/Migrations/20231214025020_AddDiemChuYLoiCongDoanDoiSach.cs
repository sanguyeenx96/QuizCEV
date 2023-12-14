using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddDiemChuYLoiCongDoanDoiSach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TTTTDiemChuY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CauhoitrinhtuthaotacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTTTDiemChuY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TTTTDiemChuY_CauHoiTrinhTuThaoTac_CauhoitrinhtuthaotacId",
                        column: x => x.CauhoitrinhtuthaotacId,
                        principalTable: "CauHoiTrinhTuThaoTac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TTTTLoiTaiCongDoan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CauhoitrinhtuthaotacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTTTLoiTaiCongDoan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TTTTLoiTaiCongDoan_CauHoiTrinhTuThaoTac_CauhoitrinhtuthaotacId",
                        column: x => x.CauhoitrinhtuthaotacId,
                        principalTable: "CauHoiTrinhTuThaoTac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoiTaiCongDoanDoiSach",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoiTaiCongDoanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoiTaiCongDoanDoiSach", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoiTaiCongDoanDoiSach_TTTTLoiTaiCongDoan_LoiTaiCongDoanId",
                        column: x => x.LoiTaiCongDoanId,
                        principalTable: "TTTTLoiTaiCongDoan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_LoiTaiCongDoanDoiSach_LoiTaiCongDoanId",
                table: "LoiTaiCongDoanDoiSach",
                column: "LoiTaiCongDoanId");

            migrationBuilder.CreateIndex(
                name: "IX_TTTTDiemChuY_CauhoitrinhtuthaotacId",
                table: "TTTTDiemChuY",
                column: "CauhoitrinhtuthaotacId");

            migrationBuilder.CreateIndex(
                name: "IX_TTTTLoiTaiCongDoan_CauhoitrinhtuthaotacId",
                table: "TTTTLoiTaiCongDoan",
                column: "CauhoitrinhtuthaotacId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoiTaiCongDoanDoiSach");

            migrationBuilder.DropTable(
                name: "TTTTDiemChuY");

            migrationBuilder.DropTable(
                name: "TTTTLoiTaiCongDoan");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("470f4021-29d8-4c8e-a9de-527571683d86"),
                column: "ConcurrencyStamp",
                value: "1b4522b2-05e2-496d-bf8d-09c2f95ec672");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "b31fe942-67ad-4699-a73d-0058cec54285");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fc622787-d2ca-4df7-8683-a800325ed8d3", "AQAAAAEAACcQAAAAEK9SftGOSgB4nivQVAYDZHuncPuP8lo1TJX6uODGY3jwyQZleYLv0+Mn1Kmp9y+vew==" });
        }
    }
}
