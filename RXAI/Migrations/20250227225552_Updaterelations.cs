using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class Updaterelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionTrade_TradeNames_AtcCode",
                table: "PrescriptionTrade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeNames",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionTrade_AtcCode",
                table: "PrescriptionTrade");

            migrationBuilder.AlterColumn<int>(
                name: "DispensedQuantity",
                table: "PrescriptionTrade",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PrescriptionTrade",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeNames",
                table: "TradeNames",
                columns: new[] { "AtcCode", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionTrade_AtcCode_Name",
                table: "PrescriptionTrade",
                columns: new[] { "AtcCode", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionTrade_TradeNames_AtcCode_Name",
                table: "PrescriptionTrade",
                columns: new[] { "AtcCode", "Name" },
                principalTable: "TradeNames",
                principalColumns: new[] { "AtcCode", "Name" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionTrade_TradeNames_AtcCode_Name",
                table: "PrescriptionTrade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeNames",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionTrade_AtcCode_Name",
                table: "PrescriptionTrade");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PrescriptionTrade");

            migrationBuilder.AlterColumn<int>(
                name: "DispensedQuantity",
                table: "PrescriptionTrade",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeNames",
                table: "TradeNames",
                column: "AtcCode");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionTrade_AtcCode",
                table: "PrescriptionTrade",
                column: "AtcCode");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionTrade_TradeNames_AtcCode",
                table: "PrescriptionTrade",
                column: "AtcCode",
                principalTable: "TradeNames",
                principalColumn: "AtcCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
