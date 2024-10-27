using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Mentor_Booking_System_Data.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "WalletTransactions");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "WalletTransactions",
                newName: "DateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "WalletTransactions",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
