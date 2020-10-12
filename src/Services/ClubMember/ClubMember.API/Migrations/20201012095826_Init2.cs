using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.ClubMember_Services.API.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubSubscription",
                columns: table => new
                {
                    ClubSubscriptionId = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubSubscription", x => x.ClubSubscriptionId);
                });

            migrationBuilder.CreateTable(
                name: "ClubMember",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    ClubSubscriptionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMember", x => new { x.UserId, x.ClubId });
                    table.ForeignKey(
                        name: "FK_ClubMember_ClubSubscription_ClubSubscriptionId",
                        column: x => x.ClubSubscriptionId,
                        principalTable: "ClubSubscription",
                        principalColumn: "ClubSubscriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubMember_ClubSubscriptionId",
                table: "ClubMember",
                column: "ClubSubscriptionId");
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
