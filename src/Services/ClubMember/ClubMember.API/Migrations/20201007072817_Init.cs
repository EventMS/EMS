using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.ClubMember_Services.API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubSubscription",
                columns: table => new
                {
                    ClubId = table.Column<Guid>(nullable: false),
                    NameOfSubscription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubSubscription", x => new { x.ClubId, x.NameOfSubscription });
                });

            migrationBuilder.CreateTable(
                name: "ClubMember",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    NameOfSubscription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMember", x => new { x.UserId, x.ClubId });
                    table.ForeignKey(
                        name: "FK_ClubMember_ClubSubscription_ClubId_NameOfSubscription",
                        columns: x => new { x.ClubId, x.NameOfSubscription },
                        principalTable: "ClubSubscription",
                        principalColumns: new[] { "ClubId", "NameOfSubscription" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubMember_ClubId_NameOfSubscription",
                table: "ClubMember",
                columns: new[] { "ClubId", "NameOfSubscription" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubMember");

            migrationBuilder.DropTable(
                name: "ClubSubscription");
        }
    }
}
