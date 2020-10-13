using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.ClubMember_Services.API.Migrations
{
    public partial class SimpleKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubMember_ClubSubscription_ClubSubscriptionId_ClubId",
                table: "ClubMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClubSubscription",
                table: "ClubSubscription");

            migrationBuilder.DropIndex(
                name: "IX_ClubMember_ClubSubscriptionId_ClubId",
                table: "ClubMember");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClubSubscription",
                table: "ClubSubscription",
                column: "ClubSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMember_ClubSubscriptionId",
                table: "ClubMember",
                column: "ClubSubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMember_ClubSubscription_ClubSubscriptionId",
                table: "ClubMember",
                column: "ClubSubscriptionId",
                principalTable: "ClubSubscription",
                principalColumn: "ClubSubscriptionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubMember_ClubSubscription_ClubSubscriptionId",
                table: "ClubMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClubSubscription",
                table: "ClubSubscription");

            migrationBuilder.DropIndex(
                name: "IX_ClubMember_ClubSubscriptionId",
                table: "ClubMember");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClubSubscription",
                table: "ClubSubscription",
                columns: new[] { "ClubSubscriptionId", "ClubId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClubMember_ClubSubscriptionId_ClubId",
                table: "ClubMember",
                columns: new[] { "ClubSubscriptionId", "ClubId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMember_ClubSubscription_ClubSubscriptionId_ClubId",
                table: "ClubMember",
                columns: new[] { "ClubSubscriptionId", "ClubId" },
                principalTable: "ClubSubscription",
                principalColumns: new[] { "ClubSubscriptionId", "ClubId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
