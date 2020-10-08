using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Room_Services.API.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Room_Name",
                table: "Room");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Booking",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Room_Name_RoomId",
                table: "Room",
                columns: new[] { "Name", "RoomId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Room_Name_RoomId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Booking");

            migrationBuilder.CreateIndex(
                name: "IX_Room_Name",
                table: "Room",
                column: "Name",
                unique: true);
        }
    }
}
