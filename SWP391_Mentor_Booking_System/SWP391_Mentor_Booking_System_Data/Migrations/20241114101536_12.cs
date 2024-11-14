using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Mentor_Booking_System_Data.Migrations
{
    /// <inheritdoc />
    public partial class _12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestToMoveClass_Students_StudentId",
                table: "RequestToMoveClass");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestToMoveClass_SwpClasses_ClassIdToMove",
                table: "RequestToMoveClass");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestToMoveClass_SwpClasses_CurrentClassId",
                table: "RequestToMoveClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestToMoveClass",
                table: "RequestToMoveClass");

            migrationBuilder.RenameTable(
                name: "RequestToMoveClass",
                newName: "RequestToMoveClasses");

            migrationBuilder.RenameIndex(
                name: "IX_RequestToMoveClass_StudentId",
                table: "RequestToMoveClasses",
                newName: "IX_RequestToMoveClasses_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestToMoveClass_CurrentClassId",
                table: "RequestToMoveClasses",
                newName: "IX_RequestToMoveClasses_CurrentClassId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestToMoveClass_ClassIdToMove",
                table: "RequestToMoveClasses",
                newName: "IX_RequestToMoveClasses_ClassIdToMove");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestToMoveClasses",
                table: "RequestToMoveClasses",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestToMoveClasses_Students_StudentId",
                table: "RequestToMoveClasses",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestToMoveClasses_SwpClasses_ClassIdToMove",
                table: "RequestToMoveClasses",
                column: "ClassIdToMove",
                principalTable: "SwpClasses",
                principalColumn: "SwpClassId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestToMoveClasses_SwpClasses_CurrentClassId",
                table: "RequestToMoveClasses",
                column: "CurrentClassId",
                principalTable: "SwpClasses",
                principalColumn: "SwpClassId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestToMoveClasses_Students_StudentId",
                table: "RequestToMoveClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestToMoveClasses_SwpClasses_ClassIdToMove",
                table: "RequestToMoveClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestToMoveClasses_SwpClasses_CurrentClassId",
                table: "RequestToMoveClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestToMoveClasses",
                table: "RequestToMoveClasses");

            migrationBuilder.RenameTable(
                name: "RequestToMoveClasses",
                newName: "RequestToMoveClass");

            migrationBuilder.RenameIndex(
                name: "IX_RequestToMoveClasses_StudentId",
                table: "RequestToMoveClass",
                newName: "IX_RequestToMoveClass_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestToMoveClasses_CurrentClassId",
                table: "RequestToMoveClass",
                newName: "IX_RequestToMoveClass_CurrentClassId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestToMoveClasses_ClassIdToMove",
                table: "RequestToMoveClass",
                newName: "IX_RequestToMoveClass_ClassIdToMove");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestToMoveClass",
                table: "RequestToMoveClass",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestToMoveClass_Students_StudentId",
                table: "RequestToMoveClass",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestToMoveClass_SwpClasses_ClassIdToMove",
                table: "RequestToMoveClass",
                column: "ClassIdToMove",
                principalTable: "SwpClasses",
                principalColumn: "SwpClassId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestToMoveClass_SwpClasses_CurrentClassId",
                table: "RequestToMoveClass",
                column: "CurrentClassId",
                principalTable: "SwpClasses",
                principalColumn: "SwpClassId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
