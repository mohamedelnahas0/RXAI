using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class compositkeyActivNa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "IngredientName",
                table: "TradeNames",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IngredientName",
                table: "Prescriptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveIngredients",
                table: "ActiveIngredients",
                columns: new[] { "DrugBankID", "IngredientName" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeNames_DrugBankID_IngredientName",
                table: "TradeNames",
                columns: new[] { "DrugBankID", "IngredientName" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DrugBankID_IngredientName",
                table: "Prescriptions",
                columns: new[] { "DrugBankID", "IngredientName" });

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID_IngredientName",
                table: "Prescriptions",
                columns: new[] { "DrugBankID", "IngredientName" },
                principalTable: "ActiveIngredients",
                principalColumns: new[] { "DrugBankID", "IngredientName" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID_IngredientName",
                table: "TradeNames",
                columns: new[] { "DrugBankID", "IngredientName" },
                principalTable: "ActiveIngredients",
                principalColumns: new[] { "DrugBankID", "IngredientName" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID_IngredientName",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID_IngredientName",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_TradeNames_DrugBankID_IngredientName",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DrugBankID_IngredientName",
                table: "Prescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveIngredients",
                table: "ActiveIngredients");

            migrationBuilder.DropColumn(
                name: "IngredientName",
                table: "TradeNames");

            migrationBuilder.DropColumn(
                name: "IngredientName",
                table: "Prescriptions");

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
    }
}
