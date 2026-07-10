using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountLifecycleAndBranding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "status",
                schema: "app",
                table: "accounts",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            // Backfill lifecycle status from the legacy Active flag (spec 03 §6.1):
            // active = true  -> Active (2); active = false -> Suspended (3).
            migrationBuilder.Sql(
                "UPDATE app.accounts SET status = CASE WHEN active THEN 2 ELSE 3 END;");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "statuschangedat",
                schema: "app",
                table: "accounts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "account_branding",
                schema: "app",
                columns: table => new
                {
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    displayname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    logodocumentid = table.Column<Guid>(type: "uuid", nullable: true),
                    primarycolor = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    reportheader = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_branding", x => x.accountid);
                    table.ForeignKey(
                        name: "FK_account_branding_accounts_accountid",
                        column: x => x.accountid,
                        principalSchema: "app",
                        principalTable: "accounts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_name",
                schema: "app",
                table: "accounts",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_status",
                schema: "app",
                table: "accounts",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_branding",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_accounts_name",
                schema: "app",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "IX_accounts_status",
                schema: "app",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "status",
                schema: "app",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "statuschangedat",
                schema: "app",
                table: "accounts");
        }
    }
}
