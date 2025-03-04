using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class Prescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionTrade");

            migrationBuilder.DropColumn(
                name: "DispenseDate",
                table: "Prescriptions");

            migrationBuilder.AddColumn<string>(
                name: "Dispensedmedication",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dose",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DrugBankID",
                table: "Prescriptions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Form",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prescription_Name",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuantityDispensed",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_ActiveIngredients_DrugBankID",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DrugBankID",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Dispensedmedication",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Dose",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "DrugBankID",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Form",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Prescription_Name",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "QuantityDispensed",
                table: "Prescriptions");

            migrationBuilder.AddColumn<DateTime>(
                name: "DispenseDate",
                table: "Prescriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.CreateTable(
                name: "PrescriptionTrade",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AtcCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DispenseTradeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DispensedQuantity = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_PrescriptionTrade_TradeNames_AtcCode_Name",
                        columns: x => new { x.AtcCode, x.Name },
                        principalTable: "TradeNames",
                        principalColumns: new[] { "AtcCode", "Name" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionTrade_AtcCode_Name",
                table: "PrescriptionTrade",
                columns: new[] { "AtcCode", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionTrade_PrescriptionID",
                table: "PrescriptionTrade",
                column: "PrescriptionID");
        }
    }
}
