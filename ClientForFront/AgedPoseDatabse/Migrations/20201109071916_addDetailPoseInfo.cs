using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AgedPoseDatabse.Migrations
{
    public partial class addDetailPoseInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetailPoseInfo",
                columns: table => new
                {
                    AgesInfoId = table.Column<long>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailPoseInfo", x => new { x.AgesInfoId, x.DateTime });
                    table.ForeignKey(
                        name: "FK_DetailPoseInfo_AgesInfo_AgesInfoId",
                        column: x => x.AgesInfoId,
                        principalTable: "AgesInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailPoseInfo");
        }
    }
}
