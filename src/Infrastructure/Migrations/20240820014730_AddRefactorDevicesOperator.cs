using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefactorDevicesOperator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_device_operator_devices_deviceid",
                schema: "app",
                table: "device_operator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_device_operator",
                schema: "app",
                table: "device_operator");

            migrationBuilder.DropColumn(
                name: "identifier",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "serial",
                schema: "app",
                table: "devices");

            migrationBuilder.AddColumn<long>(
                name: "id",
                schema: "app",
                table: "device_operator",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "identifier",
                schema: "app",
                table: "device_operator",
                type: "integer",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "serial",
                schema: "app",
                table: "device_operator",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                schema: "app",
                table: "credentials",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "refreshtoken",
                schema: "app",
                table: "credentials",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "key2",
                schema: "app",
                table: "credentials",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "key",
                schema: "app",
                table: "credentials",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_device_operator",
                schema: "app",
                table: "device_operator",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_device_operator_deviceid",
                schema: "app",
                table: "device_operator",
                column: "deviceid");

            migrationBuilder.AddForeignKey(
                name: "FK_device_operator_devices_deviceid",
                schema: "app",
                table: "device_operator",
                column: "deviceid",
                principalSchema: "app",
                principalTable: "devices",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_device_operator_devices_deviceid",
                schema: "app",
                table: "device_operator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_device_operator",
                schema: "app",
                table: "device_operator");

            migrationBuilder.DropIndex(
                name: "IX_device_operator_deviceid",
                schema: "app",
                table: "device_operator");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "app",
                table: "device_operator");

            migrationBuilder.DropColumn(
                name: "identifier",
                schema: "app",
                table: "device_operator");

            migrationBuilder.DropColumn(
                name: "serial",
                schema: "app",
                table: "device_operator");

            migrationBuilder.AddColumn<int>(
                name: "identifier",
                schema: "app",
                table: "devices",
                type: "integer",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "serial",
                schema: "app",
                table: "devices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                schema: "app",
                table: "credentials",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "refreshtoken",
                schema: "app",
                table: "credentials",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "key2",
                schema: "app",
                table: "credentials",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "key",
                schema: "app",
                table: "credentials",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_device_operator",
                schema: "app",
                table: "device_operator",
                columns: new[] { "deviceid", "operatorid" });

            migrationBuilder.AddForeignKey(
                name: "FK_device_operator_devices_deviceid",
                schema: "app",
                table: "device_operator",
                column: "deviceid",
                principalSchema: "app",
                principalTable: "devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
