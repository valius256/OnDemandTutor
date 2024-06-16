using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDemandTutor.Models.Migrations
{
    /// <inheritdoc />
    public partial class Add_FireBase_Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordStatus",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Uid",
                table: "Users",
                newName: "FireBaseid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FireBaseid",
                table: "Users",
                newName: "Uid");

            migrationBuilder.AddColumn<bool>(
                name: "RecordStatus",
                table: "Users",
                type: "bit",
                nullable: true);
        }
    }
}
