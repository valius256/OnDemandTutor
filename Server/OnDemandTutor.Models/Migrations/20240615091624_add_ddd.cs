using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDemandTutor.Models.Migrations
{
    /// <inheritdoc />
    public partial class add_ddd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_ReferenceId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ReferenceId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReferenceId",
                table: "Transactions",
                column: "ReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_ReferenceId",
                table: "Transactions",
                column: "ReferenceId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
