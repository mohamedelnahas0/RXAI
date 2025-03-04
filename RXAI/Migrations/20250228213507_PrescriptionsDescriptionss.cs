using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class PrescriptionsDescriptionss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Indication",
                table: "TradeNames");

            migrationBuilder.DropColumn(
                name: "SideEffect",
                table: "TradeNames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Indication",
                table: "TradeNames",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SideEffect",
                table: "TradeNames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
