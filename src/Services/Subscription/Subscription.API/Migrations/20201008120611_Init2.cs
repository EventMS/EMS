using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Subscription_Services.API.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClubSubscription_Name",
                table: "ClubSubscription");

            migrationBuilder.CreateIndex(
                name: "IX_ClubSubscription_Name_SubscriptionId",
                table: "ClubSubscription",
                columns: new[] { "Name", "SubscriptionId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClubSubscription_Name_SubscriptionId",
                table: "ClubSubscription");

            migrationBuilder.CreateIndex(
                name: "IX_ClubSubscription_Name",
                table: "ClubSubscription",
                column: "Name",
                unique: true);
        }
    }
}
