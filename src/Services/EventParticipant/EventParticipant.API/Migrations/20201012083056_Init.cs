using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.EventParticipant_Service.API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    EventType = table.Column<int>(nullable: false),
                    IsFree = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipant",
                columns: table => new
                {
                    EventParticipantId = table.Column<Guid>(nullable: false),
                    EventId1 = table.Column<Guid>(nullable: true),
                    EventId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipant", x => x.EventParticipantId);
                    table.ForeignKey(
                        name: "FK_EventParticipant_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipant_Event_EventId1",
                        column: x => x.EventId1,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipant_EventId",
                table: "EventParticipant",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipant_EventId1",
                table: "EventParticipant",
                column: "EventId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventParticipant");

            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
