using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.EventParticipant_Services.API.Migrations
{
    public partial class PublicPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Event");

            migrationBuilder.AddColumn<float>(
                name: "PublicPrice",
                table: "Event",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicPrice",
                table: "Event");

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
