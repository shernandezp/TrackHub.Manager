using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    /// <inheritdoc />
    public partial class Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transporter_group_users_UserId",
                schema: "app",
                table: "transporter_group");

            migrationBuilder.DropIndex(
                name: "IX_transporter_group_UserId",
                schema: "app",
                table: "transporter_group");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "transporter_group");

            migrationBuilder.RenameColumn(
                name: "DeviceTypeId",
                schema: "app",
                table: "devices",
                newName: "devicetypeid");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "app",
                table: "devices",
                newName: "description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "devicetypeid",
                schema: "app",
                table: "devices",
                newName: "DeviceTypeId");

            migrationBuilder.RenameColumn(
                name: "description",
                schema: "app",
                table: "devices",
                newName: "Description");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "transporter_group",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_transporter_group_UserId",
                schema: "app",
                table: "transporter_group",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_transporter_group_users_UserId",
                schema: "app",
                table: "transporter_group",
                column: "UserId",
                principalSchema: "app",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
