using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.PaymentWebhook_Services.API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentWebhook",
                columns: table => new
                {
                    PaymentWebhookId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentWebhook", x => x.PaymentWebhookId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentWebhook");
        }
    }
}
