using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    /// <inheritdoc />
    public partial class AddMinorDeviceUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                schema: "app",
                table: "transporters");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "app",
                table: "devices",
                newName: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                schema: "app",
                table: "devices",
                newName: "Name");

            migrationBuilder.AddColumn<short>(
                name: "icon",
                schema: "app",
                table: "transporters",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
