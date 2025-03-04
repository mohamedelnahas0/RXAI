using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class SKUPrimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeNames",
                table: "TradeNames");

            migrationBuilder.RenameColumn(
                name: "AtcCode",
                table: "TradeNames",
                newName: "SKUCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeNames",
                table: "TradeNames",
                column: "SKUCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeNames",
                table: "TradeNames");

            migrationBuilder.RenameColumn(
                name: "SKUCode",
                table: "TradeNames",
                newName: "AtcCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeNames",
                table: "TradeNames",
                columns: new[] { "AtcCode", "Name" });
        }
    }
}
