﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    /// <inheritdoc />
    public partial class AddTrasporterPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transporter_position",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterid = table.Column<Guid>(type: "uuid", nullable: false),
                    geometryid = table.Column<Guid>(type: "uuid", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    altitude = table.Column<double>(type: "double precision", nullable: true),
                    devicedatetime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    speed = table.Column<double>(type: "double precision", nullable: false),
                    course = table.Column<double>(type: "double precision", nullable: true),
                    eventId = table.Column<int>(type: "integer", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    attributes = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transporter_position", x => x.id);
                    table.ForeignKey(
                        name: "FK_transporter_position_transporters_transporterid",
                        column: x => x.transporterid,
                        principalSchema: "app",
                        principalTable: "transporters",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_transporter_position_transporterid",
                schema: "app",
                table: "transporter_position",
                column: "transporterid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transporter_position",
                schema: "app");
        }
    }
}