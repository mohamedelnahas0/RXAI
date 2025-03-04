using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrescriptionsQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DispensedQuantity",
                table: "PrescriptionTrade",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PrescriptionTrade",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
