using Microsoft.EntityFrameworkCore.Migrations;

namespace AgedPoseDatabse.Migrations
{
    public partial class AddRoomAlarm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAlarm",
                table: "RoomInfo",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAlarm",
                table: "RoomInfo");
        }
    }
}