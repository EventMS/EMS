using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Permission_Services.API.Migrations
{
    public partial class Rework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
