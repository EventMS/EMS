using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Permission_Services.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubAdministratorPermission",
                columns: table => new
                {
                    ClubId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubAdministratorPermission", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserAdministratorPermission",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdministratorPermission", x => new { x.UserId, x.ClubId });
                    table.ForeignKey(
                        name: "FK_UserAdministratorPermission_ClubAdministratorPermission_ClubId",
                        column: x => x.ClubId,
                        principalTable: "ClubAdministratorPermission",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAdministratorPermission_UserPermission_UserId",
                        column: x => x.UserId,
                        principalTable: "UserPermission",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAdministratorPermission_ClubId",
                table: "UserAdministratorPermission",
                column: "ClubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAdministratorPermission");

            migrationBuilder.DropTable(
                name: "ClubAdministratorPermission");

            migrationBuilder.DropTable(
                name: "UserPermission");
        }
    }
}
