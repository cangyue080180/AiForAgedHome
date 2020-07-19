using Microsoft.EntityFrameworkCore.Migrations;

namespace AgedPoseDatabse.Migrations
{
    public partial class AddColumnForServerInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "ServerInfo",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ServerInfo",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ServerInfo_Ip",
                table: "ServerInfo",
                column: "Ip",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerInfo_Name",
                table: "ServerInfo",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerInfo_Ip",
                table: "ServerInfo");

            migrationBuilder.DropIndex(
                name: "IX_ServerInfo_Name",
                table: "ServerInfo");

            migrationBuilder.DropColumn(
                name: "Ip",
                table: "ServerInfo");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ServerInfo");
        }
    }
}
