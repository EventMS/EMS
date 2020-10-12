using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Event_Services.API.Migrations
{
    public partial class EventTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "Event",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Event");
        }
    }
}
