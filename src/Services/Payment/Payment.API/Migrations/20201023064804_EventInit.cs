using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Payment_Services.API.Migrations
{
    public partial class EventInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    PublicPrice = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "EventPrice",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    ClubSubscriptionId = table.Column<Guid>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    EventId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPrice", x => new { x.EventId, x.ClubSubscriptionId });
                    table.ForeignKey(
                        name: "FK_EventPrice_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventPrice_Event_EventId1",
                        column: x => x.EventId1,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventPrice_EventId1",
                table: "EventPrice",
                column: "EventId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventPrice");

            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
