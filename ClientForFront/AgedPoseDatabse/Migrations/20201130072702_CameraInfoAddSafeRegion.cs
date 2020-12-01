using Microsoft.EntityFrameworkCore.Migrations;

namespace AgedPoseDatabse.Migrations
{
    public partial class CameraInfoAddSafeRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUseSafeRegion",
                table: "CameraInfo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LeftTopPoint",
                table: "CameraInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RightBottomPoint",
                table: "CameraInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUseSafeRegion",
                table: "CameraInfo");

            migrationBuilder.DropColumn(
                name: "LeftTopPoint",
                table: "CameraInfo");

            migrationBuilder.DropColumn(
                name: "RightBottomPoint",
                table: "CameraInfo");
        }
    }
}
