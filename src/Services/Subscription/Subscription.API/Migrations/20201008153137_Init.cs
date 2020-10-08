using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Subscription_Services.API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Club",
                columns: table => new
                {
                    ClubId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Club", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "ClubSubscription",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Price = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubSubscription", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_ClubSubscription_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubSubscription_ClubId",
                table: "ClubSubscription",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubSubscription_Name_ClubId",
                table: "ClubSubscription",
                columns: new[] { "Name", "ClubId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubSubscription");

            migrationBuilder.DropTable(
                name: "Club");
        }
    }
}
