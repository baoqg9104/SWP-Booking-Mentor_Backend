using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Mentor_Booking_System_Data.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeaderId",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LeaderStudentId",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_LeaderStudentId",
                table: "Groups",
                column: "LeaderStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Students_LeaderStudentId",
                table: "Groups",
                column: "LeaderStudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Students_LeaderStudentId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_LeaderStudentId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LeaderStudentId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Groups");
        }
    }
}
