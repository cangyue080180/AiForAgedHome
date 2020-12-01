using Microsoft.EntityFrameworkCore.Migrations;

namespace AgedPoseDatabse.Migrations
{
    public partial class ChangeCameraInfoSafeRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftTopPoint",
                table: "CameraInfo");

            migrationBuilder.DropColumn(
                name: "RightBottomPoint",
                table: "CameraInfo");

            migrationBuilder.AddColumn<int>(
                name: "LeftTopPointX",
                table: "CameraInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeftTopPointY",
                table: "CameraInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RightBottomPointX",
                table: "CameraInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RightBottomPointY",
                table: "CameraInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftTopPointX",
                table: "CameraInfo");

            migrationBuilder.DropColumn(
                name: "LeftTopPointY",
                table: "CameraInfo");

            migrationBuilder.DropColumn(
                name: "RightBottomPointX",
                table: "CameraInfo");

            migrationBuilder.DropColumn(
                name: "RightBottomPointY",
                table: "CameraInfo");

            migrationBuilder.AddColumn<int>(
                name: "LeftTopPoint",
                table: "CameraInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RightBottomPoint",
                table: "CameraInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
