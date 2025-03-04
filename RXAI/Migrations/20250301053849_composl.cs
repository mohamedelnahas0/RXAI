using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class composl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "TradeNames");

            migrationBuilder.DropTable(
                name: "ActiveIngredients");

            migrationBuilder.DropIndex(
                name: "IX_TradeNames_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "Prescriptions");

            migrationBuilder.CreateTable(
                name: "ActiveIngredientBases",
                columns: table => new
                {
                    DrugBankID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IngredientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredientBases", x => x.DrugBankID);
                });

            migrationBuilder.CreateTable(
                name: "ActiveIngredientVariants",
                columns: table => new
                {
                    DrugBankID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    StrengthUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ICDCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredientVariants", x => new { x.DrugBankID, x.Strength, x.StrengthUnit });
                    table.ForeignKey(
                        name: "FK_ActiveIngredientVariants_ActiveIngredientBases_DrugBankID",
                        column: x => x.DrugBankID,
                        principalTable: "ActiveIngredientBases",
                        principalColumn: "DrugBankID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActiveIngredientVariants_Diseases_ICDCode",
                        column: x => x.ICDCode,
                        principalTable: "Diseases",
                        principalColumn: "ICDCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeNames_DrugBankID_Strength_StrengthUnit",
                table: "TradeNames",
                columns: new[] { "DrugBankID", "Strength", "StrengthUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DrugBankID_Strength_StrengthUnit",
                table: "Prescriptions",
                columns: new[] { "DrugBankID", "Strength", "StrengthUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveIngredientVariants_ICDCode",
                table: "ActiveIngredientVariants",
                column: "ICDCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_ActiveIngredientVariants_DrugBankID_Strength_StrengthUnit",
                table: "Prescriptions",
                columns: new[] { "DrugBankID", "Strength", "StrengthUnit" },
                principalTable: "ActiveIngredientVariants",
                principalColumns: new[] { "DrugBankID", "Strength", "StrengthUnit" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeNames_ActiveIngredientVariants_DrugBankID_Strength_StrengthUnit",
                table: "TradeNames",
                columns: new[] { "DrugBankID", "Strength", "StrengthUnit" },
                principalTable: "ActiveIngredientVariants",
                principalColumns: new[] { "DrugBankID", "Strength", "StrengthUnit" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_ActiveIngredientVariants_DrugBankID_Strength_StrengthUnit",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeNames_ActiveIngredientVariants_DrugBankID_Strength_StrengthUnit",
                table: "TradeNames");

            migrationBuilder.DropTable(
                name: "ActiveIngredientVariants");

            migrationBuilder.DropTable(
                name: "ActiveIngredientBases");

            migrationBuilder.DropIndex(
                name: "IX_TradeNames_DrugBankID_Strength_StrengthUnit",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DrugBankID_Strength_StrengthUnit",
                table: "Prescriptions");

            migrationBuilder.CreateTable(
                name: "ActiveIngredients",
                columns: table => new
                {
                    DrugBankID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IngredientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    StrengthUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ICDCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredients", x => new { x.DrugBankID, x.IngredientName, x.Strength, x.StrengthUnit });
                    table.ForeignKey(
                        name: "FK_ActiveIngredients_Diseases_ICDCode",
                        column: x => x.ICDCode,
                        principalTable: "Diseases",
                        principalColumn: "ICDCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeNames_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "TradeNames",
                columns: new[] { "DrugBankID", "IngredientName", "Strength", "StrengthUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DrugBankID_IngredientName_Strength_StrengthUnit",
                table: "Prescriptions",
                columns: new[] { "DrugBankID", "IngredientName", "Strength", "StrengthUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveIngredients_ICDCode",
                table: "ActiveIngredients",
                column: "ICDCode");

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
    }
}
