using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Payment_Services.API.Migrations
{
    public partial class RemoveDuplicateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventPrice_Event_EventId1",
                table: "EventPrice");

            migrationBuilder.DropIndex(
                name: "IX_EventPrice_EventId1",
                table: "EventPrice");

            migrationBuilder.DropColumn(
                name: "EventId1",
                table: "EventPrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventId1",
                table: "EventPrice",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventPrice_EventId1",
                table: "EventPrice",
                column: "EventId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EventPrice_Event_EventId1",
                table: "EventPrice",
                column: "EventId1",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
