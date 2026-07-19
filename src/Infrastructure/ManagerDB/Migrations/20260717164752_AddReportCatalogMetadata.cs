using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReportCatalogMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "category",
                schema: "app",
                table: "reports",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                // Backfill existing catalog rows; the re-seed then assigns each report's real category.
                defaultValue: "Operations");

            migrationBuilder.AddColumn<bool>(
                name: "manageronly",
                schema: "app",
                table: "reports",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "requiredfeaturekey",
                schema: "app",
                table: "reports",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sortorder",
                schema: "app",
                table: "reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "supportspdf",
                schema: "app",
                table: "reports",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                schema: "app",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "manageronly",
                schema: "app",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "requiredfeaturekey",
                schema: "app",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "sortorder",
                schema: "app",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "supportspdf",
                schema: "app",
                table: "reports");
        }
    }
}
