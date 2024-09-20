using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    /// <inheritdoc />
    public partial class AddNabbarSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "navbar",
                schema: "app",
                table: "user_settings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "navbar",
                schema: "app",
                table: "user_settings");
        }
    }
}
