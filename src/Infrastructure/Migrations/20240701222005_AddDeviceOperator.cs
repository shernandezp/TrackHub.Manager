using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceOperator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories",
                schema: "app");

            migrationBuilder.AlterColumn<Guid>(
                name: "deviceid",
                schema: "app",
                table: "transporters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            /*migrationBuilder.AlterColumn<int>(
                name: "identifier",
                schema: "app",
                table: "devices",
                type: "integer",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);*/

            migrationBuilder.Sql("ALTER TABLE app.devices ALTER COLUMN identifier TYPE integer USING identifier::integer");

            migrationBuilder.AddColumn<string>(
                name: "serial",
                schema: "app",
                table: "devices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiration",
                schema: "app",
                table: "credentials",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "device_operator",
                schema: "app",
                columns: table => new
                {
                    deviceid = table.Column<Guid>(type: "uuid", nullable: false),
                    operatorid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_operator", x => new { x.deviceid, x.operatorid });
                    table.ForeignKey(
                        name: "FK_device_operator_devices_deviceid",
                        column: x => x.deviceid,
                        principalSchema: "app",
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_device_operator_operators_operatorid",
                        column: x => x.operatorid,
                        principalSchema: "app",
                        principalTable: "operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_device_operator_operatorid",
                schema: "app",
                table: "device_operator",
                column: "operatorid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device_operator",
                schema: "app");

            migrationBuilder.DropColumn(
                name: "serial",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiration",
                schema: "app",
                table: "credentials");

            migrationBuilder.AlterColumn<Guid>(
                name: "deviceid",
                schema: "app",
                table: "transporters",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "identifier",
                schema: "app",
                table: "devices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });
        }
    }
}
