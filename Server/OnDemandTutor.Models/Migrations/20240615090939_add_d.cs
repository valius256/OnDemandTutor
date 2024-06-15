using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDemandTutor.Models.Migrations
{
    /// <inheritdoc />
    public partial class add_d : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Slots_CreatedById",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "SlotId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SlotId",
                table: "Transactions",
                column: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Slots_SlotId",
                table: "Transactions",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Slots_SlotId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SlotId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Slots_CreatedById",
                table: "Transactions",
                column: "CreatedById",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
