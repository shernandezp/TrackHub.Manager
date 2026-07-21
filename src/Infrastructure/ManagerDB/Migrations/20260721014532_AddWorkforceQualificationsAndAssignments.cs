using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkforceQualificationsAndAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "driver_qualifications",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    driverid = table.Column<Guid>(type: "uuid", nullable: false),
                    qualificationtype = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    number = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    issuedat = table.Column<DateOnly>(type: "date", nullable: true),
                    expiresat = table.Column<DateOnly>(type: "date", nullable: true),
                    issuingauthority = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    documentid = table.Column<Guid>(type: "uuid", nullable: true),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driver_qualifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "driver_transporter_assignments",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    driverid = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterid = table.Column<Guid>(type: "uuid", nullable: false),
                    startsat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    endsat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    assignmenttype = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    createdbyprincipal = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driver_transporter_assignments", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_driver_qualifications_accountid_driverid",
                schema: "app",
                table: "driver_qualifications",
                columns: new[] { "accountid", "driverid" });

            migrationBuilder.CreateIndex(
                name: "IX_driver_qualifications_accountid_expiresat",
                schema: "app",
                table: "driver_qualifications",
                columns: new[] { "accountid", "expiresat" });

            migrationBuilder.CreateIndex(
                name: "ix_driver_assignments_account_driver_status",
                schema: "app",
                table: "driver_transporter_assignments",
                columns: new[] { "accountid", "driverid", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_driver_assignments_account_transporter_status",
                schema: "app",
                table: "driver_transporter_assignments",
                columns: new[] { "accountid", "transporterid", "status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driver_qualifications",
                schema: "app");

            migrationBuilder.DropTable(
                name: "driver_transporter_assignments",
                schema: "app");
        }
    }
}
