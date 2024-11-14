using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Mentor_Booking_System_Data.Migrations
{
    /// <inheritdoc />
    public partial class _11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestToMoveClass",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentClassId = table.Column<int>(type: "int", nullable: false),
                    ClassIdToMove = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestToMoveClass", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_RequestToMoveClass_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestToMoveClass_SwpClasses_ClassIdToMove",
                        column: x => x.ClassIdToMove,
                        principalTable: "SwpClasses",
                        principalColumn: "SwpClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestToMoveClass_SwpClasses_CurrentClassId",
                        column: x => x.CurrentClassId,
                        principalTable: "SwpClasses",
                        principalColumn: "SwpClassId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestToMoveClass_ClassIdToMove",
                table: "RequestToMoveClass",
                column: "ClassIdToMove");

            migrationBuilder.CreateIndex(
                name: "IX_RequestToMoveClass_CurrentClassId",
                table: "RequestToMoveClass",
                column: "CurrentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestToMoveClass_StudentId",
                table: "RequestToMoveClass",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestToMoveClass");
        }
    }
}
