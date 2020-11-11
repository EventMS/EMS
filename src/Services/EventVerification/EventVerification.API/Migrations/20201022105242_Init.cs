using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.EventVerification_Services.API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventVerification",
                columns: table => new
                {
                    EventVerificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "100, 100"),
                    EventId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventVerification", x => x.EventVerificationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventVerification");
        }
    }
}
