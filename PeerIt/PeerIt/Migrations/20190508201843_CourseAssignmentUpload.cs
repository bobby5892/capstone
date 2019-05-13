using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerIt.Migrations
{
    public partial class CourseAssignmentUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructionText",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "InstructionsUrl",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "RubricText",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "RubricUrl",
                table: "CourseAssignments");

            migrationBuilder.AddColumn<string>(
                name: "ReviewGroup",
                table: "CourseGroups",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "CourseAssignments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PFileID",
                table: "CourseAssignments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignments_PFileID",
                table: "CourseAssignments",
                column: "PFileID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_PFiles_PFileID",
                table: "CourseAssignments",
                column: "PFileID",
                principalTable: "PFiles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_PFiles_PFileID",
                table: "CourseAssignments");

            migrationBuilder.DropIndex(
                name: "IX_CourseAssignments_PFileID",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "ReviewGroup",
                table: "CourseGroups");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "PFileID",
                table: "CourseAssignments");

            migrationBuilder.AddColumn<string>(
                name: "InstructionText",
                table: "CourseAssignments",
                maxLength: 100000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionsUrl",
                table: "CourseAssignments",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RubricText",
                table: "CourseAssignments",
                maxLength: 100000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RubricUrl",
                table: "CourseAssignments",
                maxLength: 200,
                nullable: true);
        }
    }
}
