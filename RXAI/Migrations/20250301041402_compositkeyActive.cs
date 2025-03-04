using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class compositkeyActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_TradeNames_DrugBankID",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DrugBankID",
                table: "Prescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveIngredients",
                table: "ActiveIngredients");

            migrationBuilder.AddColumn<string>(
                name: "Strength",
                table: "TradeNames",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Strength",
                table: "Prescriptions",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveIngredients",
                table: "ActiveIngredients",
                columns: new[] { "DrugBankID", "Strength" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeNames_DrugBankID_Strength",
                table: "TradeNames",
                columns: new[] { "DrugBankID", "Strength" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DrugBankID_Strength",
                table: "Prescriptions",
                columns: new[] { "DrugBankID", "Strength" });

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID_Strength",
                table: "Prescriptions",
                columns: new[] { "DrugBankID", "Strength" },
                principalTable: "ActiveIngredients",
                principalColumns: new[] { "DrugBankID", "Strength" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID_Strength",
                table: "TradeNames",
                columns: new[] { "DrugBankID", "Strength" },
                principalTable: "ActiveIngredients",
                principalColumns: new[] { "DrugBankID", "Strength" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID_Strength",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID_Strength",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_TradeNames_DrugBankID_Strength",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DrugBankID_Strength",
                table: "Prescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveIngredients",
                table: "ActiveIngredients");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "TradeNames");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "Prescriptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveIngredients",
                table: "ActiveIngredients",
                column: "DrugBankID");

            migrationBuilder.CreateIndex(
                name: "IX_TradeNames_DrugBankID",
                table: "TradeNames",
                column: "DrugBankID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DrugBankID",
                table: "Prescriptions",
                column: "DrugBankID");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID",
                table: "Prescriptions",
                column: "DrugBankID",
                principalTable: "ActiveIngredients",
                principalColumn: "DrugBankID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID",
                table: "TradeNames",
                column: "DrugBankID",
                principalTable: "ActiveIngredients",
                principalColumn: "DrugBankID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
