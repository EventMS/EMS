using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Event_Services.API.Migrations
{
    public partial class removeDuplicateInstructorClubId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Club_ClubId1",
                table: "Instructor");

            migrationBuilder.DropIndex(
                name: "IX_Instructor_ClubId1",
                table: "Instructor");

            migrationBuilder.DropColumn(
                name: "ClubId1",
                table: "Instructor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClubId1",
                table: "Instructor",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructor_ClubId1",
                table: "Instructor",
                column: "ClubId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Club_ClubId1",
                table: "Instructor",
                column: "ClubId1",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
