using Microsoft.EntityFrameworkCore.Migrations;

namespace AgedPoseDatabse.Migrations
{
    public partial class AddForeignKeyForPoseinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_PoseInfo_AgesInfo_AgesInfoId",
                table: "PoseInfo",
                column: "AgesInfoId",
                principalTable: "AgesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoseInfo_AgesInfo_AgesInfoId",
                table: "PoseInfo");
        }
    }
}
