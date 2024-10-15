using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mapskey",
                schema: "app",
                table: "account_settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "onlinetimelapse",
                schema: "app",
                table: "account_settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "storingtimelapse",
                schema: "app",
                table: "account_settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mapskey",
                schema: "app",
                table: "account_settings");

            migrationBuilder.DropColumn(
                name: "onlinetimelapse",
                schema: "app",
                table: "account_settings");

            migrationBuilder.DropColumn(
                name: "storingtimelapse",
                schema: "app",
                table: "account_settings");
        }
    }
}
