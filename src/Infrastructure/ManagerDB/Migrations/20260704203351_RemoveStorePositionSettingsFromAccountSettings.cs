using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStorePositionSettingsFromAccountSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "storelastposition",
                schema: "app",
                table: "account_settings");

            migrationBuilder.DropColumn(
                name: "storinginterval",
                schema: "app",
                table: "account_settings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "storelastposition",
                schema: "app",
                table: "account_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "storinginterval",
                schema: "app",
                table: "account_settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
