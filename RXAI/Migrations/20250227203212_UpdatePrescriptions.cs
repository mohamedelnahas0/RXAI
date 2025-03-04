using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_PrescriptionDetails_PrescriptionDetailID",
                table: "Prescriptions");

            migrationBuilder.DropTable(
                name: "PrescriptionDetails");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_PrescriptionDetailID",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "PrescriptionDetailID",
                table: "Prescriptions");

            migrationBuilder.CreateTable(
                name: "PrescriptionTrade",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    AtcCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DispensedQuantity = table.Column<int>(type: "int", nullable: true),
                    DispenseTradeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionTrade", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PrescriptionTrade_Prescriptions_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionTrade_TradeNames_AtcCode",
                        column: x => x.AtcCode,
                        principalTable: "TradeNames",
                        principalColumn: "AtcCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionTrade_AtcCode",
                table: "PrescriptionTrade",
                column: "AtcCode");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionTrade_PrescriptionID",
                table: "PrescriptionTrade",
                column: "PrescriptionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionTrade");

            migrationBuilder.AddColumn<int>(
                name: "PrescriptionDetailID",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PrescriptionDetails",
                columns: table => new
                {
                    PrescriptionDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AtcCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DispenseTradeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DispensedQuantity = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionDetails", x => x.PrescriptionDetailID);
                    table.ForeignKey(
                        name: "FK_PrescriptionDetails_TradeNames_AtcCode",
                        column: x => x.AtcCode,
                        principalTable: "TradeNames",
                        principalColumn: "AtcCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PrescriptionDetailID",
                table: "Prescriptions",
                column: "PrescriptionDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetails_AtcCode",
                table: "PrescriptionDetails",
                column: "AtcCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_PrescriptionDetails_PrescriptionDetailID",
                table: "Prescriptions",
                column: "PrescriptionDetailID",
                principalTable: "PrescriptionDetails",
                principalColumn: "PrescriptionDetailID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
