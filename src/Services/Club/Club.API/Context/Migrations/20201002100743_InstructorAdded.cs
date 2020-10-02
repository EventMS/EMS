using Microsoft.EntityFrameworkCore.Migrations;

namespace Club.API.Migrations
{
    public partial class InstructorAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorIds",
                table: "Club",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorIds",
                table: "Club");
        }
    }
}
