using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDemandTutor.Models.Migrations
{
    /// <inheritdoc />
    public partial class Tutor_Degree_Subject_link : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "TutorDegrees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TutorDegreeId",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TutorDegrees_SubjectId",
                table: "TutorDegrees",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TutorDegrees_Subjects_SubjectId",
                table: "TutorDegrees",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TutorDegrees_Subjects_SubjectId",
                table: "TutorDegrees");

            migrationBuilder.DropIndex(
                name: "IX_TutorDegrees_SubjectId",
                table: "TutorDegrees");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "TutorDegrees");

            migrationBuilder.DropColumn(
                name: "TutorDegreeId",
                table: "Subjects");
        }
    }
}
