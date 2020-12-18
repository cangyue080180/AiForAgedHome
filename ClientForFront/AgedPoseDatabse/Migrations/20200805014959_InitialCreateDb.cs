using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;
using System;

namespace AgedPoseDatabse.Migrations
{
    public partial class InitialCreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    RoomSize = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServerInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    FactoryInfo = table.Column<string>(maxLength: 100, nullable: true),
                    MaxCameraCount = table.Column<byte>(nullable: false),
                    Ip = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Password = table.Column<string>(maxLength: 16, nullable: true),
                    Authority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgesInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    ContacterName = table.Column<string>(maxLength: 20, nullable: true),
                    ContacterPhone = table.Column<string>(maxLength: 11, nullable: true),
                    NurseName = table.Column<string>(maxLength: 20, nullable: true),
                    Address = table.Column<string>(maxLength: 100, nullable: true),
                    RoomInfoId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgesInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgesInfo_RoomInfo_RoomInfoId",
                        column: x => x.RoomInfoId,
                        principalTable: "RoomInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CameraInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FactoryInfo = table.Column<string>(maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(maxLength: 15, nullable: true),
                    VideoAddress = table.Column<string>(maxLength: 100, nullable: true),
                    ServerInfoId = table.Column<long>(nullable: false),
                    RoomInfoId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameraInfo_RoomInfo_RoomInfoId",
                        column: x => x.RoomInfoId,
                        principalTable: "RoomInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CameraInfo_ServerInfo_ServerInfoId",
                        column: x => x.ServerInfoId,
                        principalTable: "ServerInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoseInfo",
                columns: table => new
                {
                    AgesInfoId = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    TimeStand = table.Column<int>(nullable: false),
                    TimeSit = table.Column<int>(nullable: false),
                    TimeLie = table.Column<int>(nullable: false),
                    TimeDown = table.Column<int>(nullable: false),
                    TimeOther = table.Column<int>(nullable: false),
                    TimeIn = table.Column<string>(maxLength: 8, nullable: true),
                    IsAlarm = table.Column<bool>(nullable: false),
                    Status = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoseInfo", x => new { x.AgesInfoId, x.Date });
                    table.ForeignKey(
                        name: "FK_PoseInfo_AgesInfo_AgesInfoId",
                        column: x => x.AgesInfoId,
                        principalTable: "AgesInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgesInfo_RoomInfoId",
                table: "AgesInfo",
                column: "RoomInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraInfo_RoomInfoId",
                table: "CameraInfo",
                column: "RoomInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraInfo_ServerInfoId",
                table: "CameraInfo",
                column: "ServerInfoId");

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

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_Name",
                table: "UserInfo",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraInfo");

            migrationBuilder.DropTable(
                name: "PoseInfo");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "ServerInfo");

            migrationBuilder.DropTable(
                name: "AgesInfo");

            migrationBuilder.DropTable(
                name: "RoomInfo");
        }
    }
}
