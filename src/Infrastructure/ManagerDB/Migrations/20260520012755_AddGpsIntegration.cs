using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGpsIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_devices_transporters_transporterid",
                schema: "app",
                table: "devices");

            migrationBuilder.DropIndex(
                name: "IX_operators_accountid",
                schema: "app",
                table: "operators");

            migrationBuilder.DropIndex(
                name: "IX_devices_transporterid",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "transporterid",
                schema: "app",
                table: "devices");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiration",
                schema: "app",
                table: "credentials",
                newName: "refreshtokenexpiration");

            migrationBuilder.AddColumn<bool>(
                name: "enabled",
                schema: "app",
                table: "operators",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "healthstatus",
                schema: "app",
                table: "operators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lastdevicesyncat",
                schema: "app",
                table: "operators",
                type: "timestamp with time zone",
                nullable: true);

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
                name: "lastmanualsyncat",
                schema: "app",
                table: "operators",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lastpositionsyncat",
                schema: "app",
                table: "operators",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lastsuccessfulsyncat",
                schema: "app",
                table: "operators",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "syncintervalminutes",
                schema: "app",
                table: "operators",
                type: "integer",
                nullable: false,
                defaultValue: 60);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "app",
                table: "devices",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                schema: "app",
                table: "devices",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                schema: "app",
                table: "devices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "app",
                table: "devices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModified",
                schema: "app",
                table: "devices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "app",
                table: "devices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "detectedstatus",
                schema: "app",
                table: "devices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "firstseenat",
                schema: "app",
                table: "devices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ignoredat",
                schema: "app",
                table: "devices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lastassignedat",
                schema: "app",
                table: "devices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lastseenat",
                schema: "app",
                table: "devices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lastsyncedat",
                schema: "app",
                table: "devices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "providerdisplayname",
                schema: "app",
                table: "devices",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "providermetadatahash",
                schema: "app",
                table: "devices",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "providerstatus",
                schema: "app",
                table: "devices",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "credentialversion",
                schema: "app",
                table: "credentials",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "rotatedat",
                schema: "app",
                table: "credentials",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rotatedbyprincipalid",
                schema: "app",
                table: "credentials",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rotatedbyprincipaltype",
                schema: "app",
                table: "credentials",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "operator_health_checks",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    operatorid = table.Column<Guid>(type: "uuid", nullable: false),
                    checktype = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    latencyms = table.Column<int>(type: "integer", nullable: true),
                    startedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    errorcode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    errormessage = table.Column<string>(type: "text", nullable: true),
                    retrycount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    correlationid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operator_health_checks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "operator_sync_runs",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    operatorid = table.Column<Guid>(type: "uuid", nullable: false),
                    triggertype = table.Column<int>(type: "integer", nullable: false),
                    result = table.Column<int>(type: "integer", nullable: false),
                    startedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    devicesseen = table.Column<int>(type: "integer", nullable: false),
                    devicesadded = table.Column<int>(type: "integer", nullable: false),
                    devicesupdated = table.Column<int>(type: "integer", nullable: false),
                    devicesremoved = table.Column<int>(type: "integer", nullable: false),
                    devicesignored = table.Column<int>(type: "integer", nullable: false),
                    positionsread = table.Column<int>(type: "integer", nullable: false),
                    positionsaccepted = table.Column<int>(type: "integer", nullable: false),
                    positionsrejected = table.Column<int>(type: "integer", nullable: false),
                    errorcode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    errormessage = table.Column<string>(type: "text", nullable: true),
                    correlationid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operator_sync_runs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transporter_device_assignments",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterid = table.Column<Guid>(type: "uuid", nullable: false),
                    deviceid = table.Column<Guid>(type: "uuid", nullable: false),
                    effectivefrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    effectiveto = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    isprimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    assignmentreason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    createdbyprincipaltype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    createdbyprincipalid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transporter_device_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_transporter_device_assignments_devices_deviceid",
                        column: x => x.deviceid,
                        principalSchema: "app",
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transporter_device_assignments_transporters_transporterid",
                        column: x => x.transporterid,
                        principalSchema: "app",
                        principalTable: "transporters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transporter_position_history",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    operatorid = table.Column<Guid>(type: "uuid", nullable: false),
                    deviceid = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterid = table.Column<Guid>(type: "uuid", nullable: false),
                    sourcetimestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    receivedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    altitude = table.Column<double>(type: "double precision", nullable: true),
                    speed = table.Column<double>(type: "double precision", nullable: false),
                    course = table.Column<double>(type: "double precision", nullable: true),
                    eventid = table.Column<int>(type: "integer", nullable: true),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    city = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    state = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    country = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    attributes = table.Column<string>(type: "text", nullable: true),
                    idempotencykey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transporter_position_history", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_operators_accountid_enabled_protocoltype",
                schema: "app",
                table: "operators",
                columns: new[] { "accountid", "enabled", "protocoltype" });

            migrationBuilder.CreateIndex(
                name: "IX_operators_accountid_healthstatus",
                schema: "app",
                table: "operators",
                columns: new[] { "accountid", "healthstatus" });

            migrationBuilder.CreateIndex(
                name: "IX_devices_accountid_operatorid_detectedstatus_lastsyncedat",
                schema: "app",
                table: "devices",
                columns: new[] { "accountid", "operatorid", "detectedstatus", "lastsyncedat" });

            migrationBuilder.CreateIndex(
                name: "IX_devices_accountid_operatorid_identifier",
                schema: "app",
                table: "devices",
                columns: new[] { "accountid", "operatorid", "identifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operator_health_checks_accountid_operatorid_startedat",
                schema: "app",
                table: "operator_health_checks",
                columns: new[] { "accountid", "operatorid", "startedat" });

            migrationBuilder.CreateIndex(
                name: "IX_operator_sync_runs_accountid_operatorid_startedat",
                schema: "app",
                table: "operator_sync_runs",
                columns: new[] { "accountid", "operatorid", "startedat" });

            migrationBuilder.CreateIndex(
                name: "IX_operator_sync_runs_accountid_startedat",
                schema: "app",
                table: "operator_sync_runs",
                columns: new[] { "accountid", "startedat" });

            migrationBuilder.CreateIndex(
                name: "IX_transporter_device_assignments_accountid_deviceid_effective~",
                schema: "app",
                table: "transporter_device_assignments",
                columns: new[] { "accountid", "deviceid", "effectivefrom" });

            migrationBuilder.CreateIndex(
                name: "IX_transporter_device_assignments_accountid_transporterid_effe~",
                schema: "app",
                table: "transporter_device_assignments",
                columns: new[] { "accountid", "transporterid", "effectivefrom" });

            migrationBuilder.CreateIndex(
                name: "ix_transporter_device_assignments_active_device",
                schema: "app",
                table: "transporter_device_assignments",
                columns: new[] { "accountid", "deviceid" },
                unique: true,
                filter: "status = 0");

            migrationBuilder.CreateIndex(
                name: "IX_transporter_device_assignments_deviceid",
                schema: "app",
                table: "transporter_device_assignments",
                column: "deviceid");

            migrationBuilder.CreateIndex(
                name: "IX_transporter_device_assignments_transporterid",
                schema: "app",
                table: "transporter_device_assignments",
                column: "transporterid");

            migrationBuilder.CreateIndex(
                name: "IX_transporter_position_history_accountid_operatorid_sourcetim~",
                schema: "app",
                table: "transporter_position_history",
                columns: new[] { "accountid", "operatorid", "sourcetimestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_transporter_position_history_accountid_transporterid_source~",
                schema: "app",
                table: "transporter_position_history",
                columns: new[] { "accountid", "transporterid", "sourcetimestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_transporter_position_history_idempotencykey",
                schema: "app",
                table: "transporter_position_history",
                column: "idempotencykey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "operator_health_checks",
                schema: "app");

            migrationBuilder.DropTable(
                name: "operator_sync_runs",
                schema: "app");

            migrationBuilder.DropTable(
                name: "transporter_device_assignments",
                schema: "app");

            migrationBuilder.DropTable(
                name: "transporter_position_history",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_operators_accountid_enabled_protocoltype",
                schema: "app",
                table: "operators");

            migrationBuilder.DropIndex(
                name: "IX_operators_accountid_healthstatus",
                schema: "app",
                table: "operators");

            migrationBuilder.DropIndex(
                name: "IX_devices_accountid_operatorid_detectedstatus_lastsyncedat",
                schema: "app",
                table: "devices");

            migrationBuilder.DropIndex(
                name: "IX_devices_accountid_operatorid_identifier",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "enabled",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "healthstatus",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "lastdevicesyncat",
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
                name: "lastmanualsyncat",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "lastpositionsyncat",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "lastsuccessfulsyncat",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "syncintervalminutes",
                schema: "app",
                table: "operators");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "detectedstatus",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "firstseenat",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "ignoredat",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "lastassignedat",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "lastseenat",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "lastsyncedat",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "providerdisplayname",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "providermetadatahash",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "providerstatus",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "credentialversion",
                schema: "app",
                table: "credentials");

            migrationBuilder.DropColumn(
                name: "rotatedat",
                schema: "app",
                table: "credentials");

            migrationBuilder.DropColumn(
                name: "rotatedbyprincipalid",
                schema: "app",
                table: "credentials");

            migrationBuilder.DropColumn(
                name: "rotatedbyprincipaltype",
                schema: "app",
                table: "credentials");

            migrationBuilder.RenameColumn(
                name: "refreshtokenexpiration",
                schema: "app",
                table: "credentials",
                newName: "RefreshTokenExpiration");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "app",
                table: "devices",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                schema: "app",
                table: "devices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "transporterid",
                schema: "app",
                table: "devices",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_operators_accountid",
                schema: "app",
                table: "operators",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_devices_transporterid",
                schema: "app",
                table: "devices",
                column: "transporterid");

            migrationBuilder.AddForeignKey(
                name: "FK_devices_transporters_transporterid",
                schema: "app",
                table: "devices",
                column: "transporterid",
                principalSchema: "app",
                principalTable: "transporters",
                principalColumn: "id");
        }
    }
}
