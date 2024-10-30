using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    /// <inheritdoc />
    public partial class FixIntervalNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "storingtimelapse",
                schema: "app",
                table: "account_settings",
                newName: "storinginterval");

            migrationBuilder.RenameColumn(
                name: "refreshmaptimer",
                schema: "app",
                table: "account_settings",
                newName: "refreshmapinterval");

            migrationBuilder.RenameColumn(
                name: "onlinetimelapse",
                schema: "app",
                table: "account_settings",
                newName: "onlineinterval");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "storinginterval",
                schema: "app",
                table: "account_settings",
                newName: "storingtimelapse");

            migrationBuilder.RenameColumn(
                name: "refreshmapinterval",
                schema: "app",
                table: "account_settings",
                newName: "refreshmaptimer");

            migrationBuilder.RenameColumn(
                name: "onlineinterval",
                schema: "app",
                table: "account_settings",
                newName: "onlinetimelapse");
        }
    }
}
