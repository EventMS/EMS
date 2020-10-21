using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Subscription_Services.API.Migrations
{
    public partial class StripeInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StribePriceId",
                table: "ClubSubscription",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StribePriceId",
                table: "ClubSubscription");
        }
    }
}
