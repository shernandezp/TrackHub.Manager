using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlatformAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "platform_announcements",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    messageen = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    messagees = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    severity = table.Column<int>(type: "integer", nullable: false),
                    startsat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    endsat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_announcements", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_platform_announcements_active_startsat_endsat",
                schema: "app",
                table: "platform_announcements",
                columns: new[] { "active", "startsat", "endsat" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "platform_announcements",
                schema: "app");
        }
    }
}
