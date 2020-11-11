using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Club_Service.API.Migrations
{
    public partial class RemovedInstructor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorIds",
                table: "Club");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorIds",
                table: "Club",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
