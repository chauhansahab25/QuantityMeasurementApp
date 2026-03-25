using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementWebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuantityMeasurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstValue = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    FirstUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecondValue = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    SecondUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Result = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    MeasurementType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsError = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResultString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityMeasurements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuantityMeasurements_CreatedAt",
                table: "QuantityMeasurements",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_QuantityMeasurements_IsError",
                table: "QuantityMeasurements",
                column: "IsError");

            migrationBuilder.CreateIndex(
                name: "IX_QuantityMeasurements_MeasurementType",
                table: "QuantityMeasurements",
                column: "MeasurementType");

            migrationBuilder.CreateIndex(
                name: "IX_QuantityMeasurements_Operation",
                table: "QuantityMeasurements",
                column: "Operation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuantityMeasurements");
        }
    }
}
