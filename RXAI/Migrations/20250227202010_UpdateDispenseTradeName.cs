using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDispenseTradeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DispenseDate",
                table: "PrescriptionDetails");

            migrationBuilder.AddColumn<string>(
                name: "DispenseTradeName",
                table: "PrescriptionDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ICDCode",
                table: "Diseases",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DispenseTradeName",
                table: "PrescriptionDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "DispenseDate",
                table: "PrescriptionDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ICDCode",
                table: "Diseases",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
