using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    /// <inheritdoc />
    public partial class FixPgDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "devicedatetime",
                schema: "app",
                table: "transporter_position",
                newName: "datetime");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "offset",
                schema: "app",
                table: "transporter_position",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "offset",
                schema: "app",
                table: "transporter_position");

            migrationBuilder.RenameColumn(
                name: "datetime",
                schema: "app",
                table: "transporter_position",
                newName: "devicedatetime");
        }
    }
}
