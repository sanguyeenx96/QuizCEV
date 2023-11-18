using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class UpdateThutu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QCorrectAns",
                table: "CauHoiTrinhTuThaoTac");

            migrationBuilder.AddColumn<int>(
                name: "ThuTu",
                table: "CauHoiTrinhTuThaoTac",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThuTu",
                table: "CauHoiTrinhTuThaoTac");

            migrationBuilder.AddColumn<string>(
                name: "QCorrectAns",
                table: "CauHoiTrinhTuThaoTac",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
