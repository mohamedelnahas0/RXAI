using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class AddSiease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveIngredientVariants_Diseases_ICDCode",
                table: "ActiveIngredientVariants");

            migrationBuilder.DropIndex(
                name: "IX_ActiveIngredientVariants_ICDCode",
                table: "ActiveIngredientVariants");

            migrationBuilder.DropColumn(
                name: "ICDCode",
                table: "ActiveIngredientVariants");

            migrationBuilder.AddColumn<string>(
                name: "ICDCode",
                table: "ActiveIngredientBases",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveIngredientBases_ICDCode",
                table: "ActiveIngredientBases",
                column: "ICDCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveIngredientBases_Diseases_ICDCode",
                table: "ActiveIngredientBases",
                column: "ICDCode",
                principalTable: "Diseases",
                principalColumn: "ICDCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveIngredientBases_Diseases_ICDCode",
                table: "ActiveIngredientBases");

            migrationBuilder.DropIndex(
                name: "IX_ActiveIngredientBases_ICDCode",
                table: "ActiveIngredientBases");

            migrationBuilder.DropColumn(
                name: "ICDCode",
                table: "ActiveIngredientBases");

            migrationBuilder.AddColumn<string>(
                name: "ICDCode",
                table: "ActiveIngredientVariants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveIngredientVariants_ICDCode",
                table: "ActiveIngredientVariants",
                column: "ICDCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveIngredientVariants_Diseases_ICDCode",
                table: "ActiveIngredientVariants",
                column: "ICDCode",
                principalTable: "Diseases",
                principalColumn: "ICDCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
