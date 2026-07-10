using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropOperatorHealthRollupColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_operators_accountid_healthstatus",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "healthstatus",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "lastfailedsyncat",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "lastfailurecode",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "lastfailuremessage",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "lastlatencyms",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "lastpositionsyncat",
                schema: "app",
                table: "operators");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "healthstatus",
                schema: "app",
                table: "operators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lastfailedsyncat",
                schema: "app",
                table: "operators",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastfailurecode",
                schema: "app",
                table: "operators",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastfailuremessage",
                schema: "app",
                table: "operators",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "lastlatencyms",
                schema: "app",
                table: "operators",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lastpositionsyncat",
                schema: "app",
                table: "operators",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_operators_accountid_healthstatus",
                schema: "app",
                table: "operators",
                columns: new[] { "accountid", "healthstatus" });
        }
    }
}
