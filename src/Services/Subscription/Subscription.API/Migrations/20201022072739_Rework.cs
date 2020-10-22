using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Subscription_Services.API.Migrations
{
    public partial class Rework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StribePriceId",
                table: "ClubSubscription");

            migrationBuilder.DropColumn(
                name: "StribeProductId",
                table: "ClubSubscription");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StribePriceId",
                table: "ClubSubscription",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StribeProductId",
                table: "ClubSubscription",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
