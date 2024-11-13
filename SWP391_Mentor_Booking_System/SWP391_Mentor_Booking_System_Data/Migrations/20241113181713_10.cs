using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Mentor_Booking_System_Data.Migrations
{
    /// <inheritdoc />
    public partial class _10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SwpClassId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SwpClassId",
                table: "Mentors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_SwpClassId",
                table: "Students",
                column: "SwpClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentors_SwpClassId",
                table: "Mentors",
                column: "SwpClassId",
                unique: true,
                filter: "[SwpClassId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_SwpClasses_SwpClassId",
                table: "Mentors",
                column: "SwpClassId",
                principalTable: "SwpClasses",
                principalColumn: "SwpClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_SwpClasses_SwpClassId",
                table: "Students",
                column: "SwpClassId",
                principalTable: "SwpClasses",
                principalColumn: "SwpClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_SwpClasses_SwpClassId",
                table: "Mentors");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_SwpClasses_SwpClassId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_SwpClassId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Mentors_SwpClassId",
                table: "Mentors");

            migrationBuilder.DropColumn(
                name: "SwpClassId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SwpClassId",
                table: "Mentors");
        }
    }
}
