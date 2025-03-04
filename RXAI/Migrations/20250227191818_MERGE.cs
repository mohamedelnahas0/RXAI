using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class MERGE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    ICDCode = table.Column<string>(type: "nvarchar(20)", maxLength: 6, nullable: false),
                    DiseaseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.ICDCode);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PhoneNumber);
                });

            migrationBuilder.CreateTable(
                name: "ActiveIngredients",
                columns: table => new
                {
                    DrugBankID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IngredientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ICDCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    StrengthUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredients", x => x.DrugBankID);
                    table.ForeignKey(
                        name: "FK_ActiveIngredients_Diseases_ICDCode",
                        column: x => x.ICDCode,
                        principalTable: "Diseases",
                        principalColumn: "ICDCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeNames",
                columns: table => new
                {
                    AtcCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DrugBankID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Indication = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PharmaceuticalForm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SideEffect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityStock = table.Column<int>(type: "int", nullable: true),
                    ManufactureCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeNames", x => x.AtcCode);
                    table.ForeignKey(
                        name: "FK_TradeNames_ActiveIngredients_DrugBankID",
                        column: x => x.DrugBankID,
                        principalTable: "ActiveIngredients",
                        principalColumn: "DrugBankID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionDetails",
                columns: table => new
                {
                    PrescriptionDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DispensedQuantity = table.Column<int>(type: "int", nullable: true),
                    DispenseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AtcCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PrescriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DispenseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrescriptionDetailID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionID);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Patients_PhoneNumber",
                        column: x => x.PhoneNumber,
                        principalTable: "Patients",
                        principalColumn: "PhoneNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescriptions_PrescriptionDetails_PrescriptionDetailID",
                        column: x => x.PrescriptionDetailID,
                        principalTable: "PrescriptionDetails",
                        principalColumn: "PrescriptionDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveIngredients_ICDCode",
                table: "ActiveIngredients",
                column: "ICDCode");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetails_AtcCode",
                table: "PrescriptionDetails",
                column: "AtcCode");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PhoneNumber",
                table: "Prescriptions",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PrescriptionDetailID",
                table: "Prescriptions",
                column: "PrescriptionDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_TradeNames_DrugBankID",
                table: "TradeNames",
                column: "DrugBankID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "PrescriptionDetails");

            migrationBuilder.DropTable(
                name: "TradeNames");

            migrationBuilder.DropTable(
                name: "ActiveIngredients");

            migrationBuilder.DropTable(
                name: "Diseases");
        }
    }
}
