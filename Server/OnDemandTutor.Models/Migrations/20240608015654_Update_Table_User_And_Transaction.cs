using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDemandTutor.Models.Migrations
{
    /// <inheritdoc />
    public partial class Update_Table_User_And_Transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Classes_ClassId",
                table: "Slots");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Slots_SlotId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SlotId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Slots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SlotTransaction",
                columns: table => new
                {
                    SlotTransactionNavigationId = table.Column<int>(type: "int", nullable: false),
                    TransactionNavigationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotTransaction", x => new { x.SlotTransactionNavigationId, x.TransactionNavigationId });
                    table.ForeignKey(
                        name: "FK_SlotTransaction_Slots_TransactionNavigationId",
                        column: x => x.TransactionNavigationId,
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SlotTransaction_Transactions_SlotTransactionNavigationId",
                        column: x => x.SlotTransactionNavigationId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SlotTransaction_TransactionNavigationId",
                table: "SlotTransaction",
                column: "TransactionNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Classes_ClassId",
                table: "Slots",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Classes_ClassId",
                table: "Slots");

            migrationBuilder.DropTable(
                name: "SlotTransaction");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Slots");

            migrationBuilder.AddColumn<int>(
                name: "SlotId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SlotId",
                table: "Transactions",
                column: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Classes_ClassId",
                table: "Slots",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Slots_SlotId",
                table: "Transactions",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id");
        }
    }
}
