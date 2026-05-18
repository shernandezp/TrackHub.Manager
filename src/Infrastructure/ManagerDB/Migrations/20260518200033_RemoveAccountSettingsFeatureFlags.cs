using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAccountSettingsFeatureFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE app.account_settings DROP COLUMN IF EXISTS geofencing;");
            migrationBuilder.Sql("ALTER TABLE app.account_settings DROP COLUMN IF EXISTS tripmanagement;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE app.account_settings ADD COLUMN IF NOT EXISTS geofencing boolean NOT NULL DEFAULT FALSE;");
            migrationBuilder.Sql("ALTER TABLE app.account_settings ADD COLUMN IF NOT EXISTS tripmanagement boolean NOT NULL DEFAULT FALSE;");
        }
    }
}
