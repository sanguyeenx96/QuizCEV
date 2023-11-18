using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class CauHoiTrinhTuThaoTac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QCorrectAns",
                table: "CauHoiTuLuan");

            migrationBuilder.CreateTable(
                name: "CauHoiTrinhTuThaoTac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QCorrectAns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<float>(type: "real", nullable: true),
                    CauHoiTuLuanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauHoiTrinhTuThaoTac", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CauHoiTrinhTuThaoTac_CauHoiTuLuan_CauHoiTuLuanId",
                        column: x => x.CauHoiTuLuanId,
                        principalTable: "CauHoiTuLuan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CauHoiTrinhTuThaoTac_CauHoiTuLuanId",
                table: "CauHoiTrinhTuThaoTac",
                column: "CauHoiTuLuanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauHoiTrinhTuThaoTac");

            migrationBuilder.AddColumn<string>(
                name: "QCorrectAns",
                table: "CauHoiTuLuan",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
