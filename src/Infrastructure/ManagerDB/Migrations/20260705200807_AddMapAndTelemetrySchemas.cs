using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMapAndTelemetrySchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "map");

            migrationBuilder.EnsureSchema(
                name: "telemetry");

            migrationBuilder.RenameTable(
                name: "transporter_position_history",
                schema: "app",
                newName: "transporter_position_history",
                newSchema: "telemetry");

            migrationBuilder.CreateTable(
                name: "geocoding_providers",
                schema: "map",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    endpointuri = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    apikey = table.Column<string>(type: "text", nullable: true),
                    salt = table.Column<string>(type: "text", nullable: true),
                    requestspersecond = table.Column<int>(type: "integer", nullable: false),
                    timeoutseconds = table.Column<int>(type: "integer", nullable: false),
                    configurationjson = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geocoding_providers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "points_of_interest",
                schema: "map",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    color = table.Column<short>(type: "smallint", nullable: true),
                    groupid = table.Column<long>(type: "bigint", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_points_of_interest", x => x.id);
                    table.ForeignKey(
                        name: "FK_points_of_interest_accounts_accountid",
                        column: x => x.accountid,
                        principalSchema: "app",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_points_of_interest_groups_groupid",
                        column: x => x.groupid,
                        principalSchema: "app",
                        principalTable: "groups",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_geocoding_providers_single_active",
                schema: "map",
                table: "geocoding_providers",
                column: "active",
                unique: true,
                filter: "active = true");

            migrationBuilder.CreateIndex(
                name: "IX_points_of_interest_accountid",
                schema: "map",
                table: "points_of_interest",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_points_of_interest_accountid_active",
                schema: "map",
                table: "points_of_interest",
                columns: new[] { "accountid", "active" });

            migrationBuilder.CreateIndex(
                name: "IX_points_of_interest_groupid",
                schema: "map",
                table: "points_of_interest",
                column: "groupid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "geocoding_providers",
                schema: "map");

            migrationBuilder.DropTable(
                name: "points_of_interest",
                schema: "map");

            migrationBuilder.RenameTable(
                name: "transporter_position_history",
                schema: "telemetry",
                newName: "transporter_position_history",
                newSchema: "app");
        }
    }
}
