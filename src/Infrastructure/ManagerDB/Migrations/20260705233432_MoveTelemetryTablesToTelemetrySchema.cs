using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveTelemetryTablesToTelemetrySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "transporter_position",
                schema: "app",
                newName: "transporter_position",
                newSchema: "telemetry");

            migrationBuilder.RenameTable(
                name: "operator_sync_runs",
                schema: "app",
                newName: "operator_sync_runs",
                newSchema: "telemetry");

            migrationBuilder.RenameTable(
                name: "operator_health_checks",
                schema: "app",
                newName: "operator_health_checks",
                newSchema: "telemetry");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "transporter_position",
                schema: "telemetry",
                newName: "transporter_position",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "operator_sync_runs",
                schema: "telemetry",
                newName: "operator_sync_runs",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "operator_health_checks",
                schema: "telemetry",
                newName: "operator_health_checks",
                newSchema: "app");
        }
    }
}
