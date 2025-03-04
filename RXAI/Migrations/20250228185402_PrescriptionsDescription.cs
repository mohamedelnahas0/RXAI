using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class PrescriptionsDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Prescription_Name",
                table: "Prescriptions",
                newName: "Prescription_Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Prescription_Description",
                table: "Prescriptions",
                newName: "Prescription_Name");
        }
    }
}
