using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Mentor_Booking_System_Data.Migrations
{
    /// <inheritdoc />
    public partial class _9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Students_LeaderId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_LeaderId",
                table: "Groups");

            migrationBuilder.AlterColumn<string>(
                name: "LeaderId",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LeaderId",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_LeaderId",
                table: "Groups",
                column: "LeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Students_LeaderId",
                table: "Groups",
                column: "LeaderId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
