using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class compositkeyActiALl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Strength",
                table: "TradeNames",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StrengthUnit",
                table: "TradeNames",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Strength",
                table: "Prescriptions",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StrengthUnit",
                table: "Prescriptions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveIngredients",
                table: "ActiveIngredients",
                columns: new[] { "DrugBankID", "IngredientName", "Strength", "StrengthUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeNames_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "TradeNames",
                columns: new[] { "DrugBankID", "IngredientName", "Strength", "StrengthUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "Prescriptions",
                columns: new[] { "DrugBankID", "IngredientName", "Strength", "StrengthUnit" });

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "Prescriptions",
                columns: new[] { "DrugBankID", "IngredientName", "Strength", "StrengthUnit" },
                principalTable: "ActiveIngredients",
                principalColumns: new[] { "DrugBankID", "IngredientName", "Strength", "StrengthUnit" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "TradeNames",
                columns: new[] { "DrugBankID", "IngredientName", "Strength", "StrengthUnit" },
                principalTable: "ActiveIngredients",
                principalColumns: new[] { "DrugBankID", "IngredientName", "Strength", "StrengthUnit" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_TradeNames_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "Prescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveIngredients",
                table: "ActiveIngredients");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "TradeNames");

            migrationBuilder.DropColumn(
                name: "StrengthUnit",
                table: "TradeNames");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "StrengthUnit",
                table: "Prescriptions");

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
    }
}
