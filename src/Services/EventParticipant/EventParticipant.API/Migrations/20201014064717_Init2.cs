using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.EventParticipant_Services.API.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "Event");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "Event",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
