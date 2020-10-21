using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Subscription_Services.API.Migrations
{
    public partial class StripeInit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StribeProductId",
                table: "ClubSubscription",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StribeProductId",
                table: "ClubSubscription");
        }
    }
}
