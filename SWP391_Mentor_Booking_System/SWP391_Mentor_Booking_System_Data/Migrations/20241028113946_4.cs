using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Mentor_Booking_System_Data.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingSlots_MentorSkills_MentorSkillId",
                table: "BookingSlots");

            migrationBuilder.DropIndex(
                name: "IX_BookingSlots_MentorSkillId",
                table: "BookingSlots");

            migrationBuilder.DropColumn(
                name: "MentorSkillId",
                table: "BookingSlots");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BookingSkills",
                columns: table => new
                {
                    BookingSkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingSlotId = table.Column<int>(type: "int", nullable: false),
                    MentorSkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingSkills", x => x.BookingSkillId);
                    table.ForeignKey(
                        name: "FK_BookingSkills_BookingSlots_BookingSlotId",
                        column: x => x.BookingSlotId,
                        principalTable: "BookingSlots",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingSkills_MentorSkills_MentorSkillId",
                        column: x => x.MentorSkillId,
                        principalTable: "MentorSkills",
                        principalColumn: "MentorSkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingSkills_BookingSlotId",
                table: "BookingSkills",
                column: "BookingSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSkills_MentorSkillId",
                table: "BookingSkills",
                column: "MentorSkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingSkills");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "WalletTransactions");

            migrationBuilder.AddColumn<int>(
                name: "MentorSkillId",
                table: "BookingSlots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookingSlots_MentorSkillId",
                table: "BookingSlots",
                column: "MentorSkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingSlots_MentorSkills_MentorSkillId",
                table: "BookingSlots",
                column: "MentorSkillId",
                principalTable: "MentorSkills",
                principalColumn: "MentorSkillId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
