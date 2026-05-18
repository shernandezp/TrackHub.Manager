using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlatformServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_accountid",
                schema: "app",
                table: "users");

            migrationBuilder.AddColumn<Guid>(
                name: "accountid",
                schema: "app",
                table: "transporters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "accountid",
                schema: "app",
                table: "devices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            // Backfill transporters.accountid from the historical transporter -> transporter_group -> groups -> accounts chain.
            // Existing transporters were tenant-scoped indirectly through their owning groups; account-scoped services require the
            // tenant column to be populated directly so that account-cascade foreign keys and tenant filters work for existing data.
            migrationBuilder.Sql(@"
                UPDATE app.transporters AS t
                SET accountid = sub.accountid
                FROM (
                    SELECT tg.transporterid, MIN(g.accountid) AS accountid
                    FROM app.transporter_group tg
                    JOIN app.groups g ON g.id = tg.groupid
                    GROUP BY tg.transporterid
                ) AS sub
                WHERE sub.transporterid = t.id
                  AND t.accountid = '00000000-0000-0000-0000-000000000000';
            ");

            // Backfill devices.accountid from the related transporter (devices reference exactly one transporter).
            migrationBuilder.Sql(@"
                UPDATE app.devices AS d
                SET accountid = t.accountid
                FROM app.transporters AS t
                WHERE d.transporterid = t.id
                  AND d.accountid = '00000000-0000-0000-0000-000000000000';
            ");

            // Orphans (transporters with no group, or devices whose transporter is unknown) cannot resolve to an account.
            // Removing them is preferable to leaving Guid.Empty rows that would violate the new FK to accounts and become
            // invisible to tenant-scoped queries. Operators should reconcile data prior to deploying these platform services if
            // they need to preserve such rows; the orphan deletes cascade through downstream references.
            migrationBuilder.Sql(@"
                DELETE FROM app.devices
                WHERE accountid = '00000000-0000-0000-0000-000000000000';
            ");
            migrationBuilder.Sql(@"
                DELETE FROM app.transporters
                WHERE accountid = '00000000-0000-0000-0000-000000000000';
            ");

            migrationBuilder.CreateTable(
                name: "account_features",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    featurekey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    tier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    source = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    effectivefrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    effectiveto = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    configurationjson = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_features", x => x.id);
                    table.ForeignKey(
                        name: "FK_account_features_accounts_accountid",
                        column: x => x.accountid,
                        principalSchema: "app",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_support_grants",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    supportuserid = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ticketreference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    approvedby = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    approvedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    accesslevel = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    startsat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    endsat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    revokedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    revokedby = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_support_grants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "alert_events",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    eventtype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    severity = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sourcemodule = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    resourcetype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    resourceid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    firstseenat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    lastseenat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    payloadjson = table.Column<string>(type: "text", nullable: true),
                    deduplicationkey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audit_events",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    actortype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    actorid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    action = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    resourcetype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    resourceid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    result = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    oldvaluesjson = table.Column<string>(type: "text", nullable: true),
                    newvaluesjson = table.Column<string>(type: "text", nullable: true),
                    reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ipaddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    useragent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    correlationid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    occurredat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "background_job_runs",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    jobkey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: true),
                    resourcekey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    idempotencykey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    attempts = table.Column<int>(type: "integer", nullable: false),
                    startedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    errorcode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    errormessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_background_job_runs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "documents",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    ownerentitytype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ownerentityid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    uploadedbyprincipaltype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    uploadedbyprincipalid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    storageprovider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    storagekey = table.Column<string>(type: "text", nullable: false),
                    contenttype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sizebytes = table.Column<long>(type: "bigint", nullable: false),
                    sha256hash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    classification = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    expiresat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    visibilityscope = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    scanstatus = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drivers",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    phone = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    documenttype = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    documentnumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    employeecode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    licensenumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    licenseexpiresat = table.Column<DateOnly>(type: "date", nullable: true),
                    defaulttransporterid = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drivers", x => x.id);
                    table.ForeignKey(
                        name: "FK_drivers_accounts_accountid",
                        column: x => x.accountid,
                        principalSchema: "app",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification_deliveries",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    notificationruleid = table.Column<Guid>(type: "uuid", nullable: true),
                    alerteventid = table.Column<Guid>(type: "uuid", nullable: true),
                    channel = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    recipientprincipaltype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    recipient = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    attempts = table.Column<int>(type: "integer", nullable: false),
                    providermessageid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    error = table.Column<string>(type: "text", nullable: true),
                    sentat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    readat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_deliveries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_rules",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    rulekey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ruletype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    triggerevent = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    recipientselector = table.Column<string>(type: "text", nullable: false),
                    channelsjson = table.Column<string>(type: "text", nullable: false),
                    throttlingjson = table.Column<string>(type: "text", nullable: true),
                    configurationjson = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "public_link_grants",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    resourcetype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    resourceid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    scopes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    purpose = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    subjecttokenidhash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    expiresat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    revokedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    revokedby = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    createdbyprincipalid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    accesscount = table.Column<int>(type: "integer", nullable: false),
                    lastaccessedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_public_link_grants", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_accountid_username",
                schema: "app",
                table: "users",
                columns: new[] { "accountid", "username" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transporters_accountid",
                schema: "app",
                table: "transporters",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_devices_accountid",
                schema: "app",
                table: "devices",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_account_features_accountid_featurekey",
                schema: "app",
                table: "account_features",
                columns: new[] { "accountid", "featurekey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_account_support_grants_accountid_supportuserid_startsat_end~",
                schema: "app",
                table: "account_support_grants",
                columns: new[] { "accountid", "supportuserid", "startsat", "endsat" });

            migrationBuilder.CreateIndex(
                name: "IX_alert_events_accountid_status_lastseenat",
                schema: "app",
                table: "alert_events",
                columns: new[] { "accountid", "status", "lastseenat" });

            migrationBuilder.CreateIndex(
                name: "IX_alert_events_deduplicationkey",
                schema: "app",
                table: "alert_events",
                column: "deduplicationkey");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_accountid_occurredat",
                schema: "app",
                table: "audit_events",
                columns: new[] { "accountid", "occurredat" });

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_correlationid",
                schema: "app",
                table: "audit_events",
                column: "correlationid");

            migrationBuilder.CreateIndex(
                name: "IX_background_job_runs_accountid_startedat",
                schema: "app",
                table: "background_job_runs",
                columns: new[] { "accountid", "startedat" });

            migrationBuilder.CreateIndex(
                name: "IX_background_job_runs_jobkey_idempotencykey",
                schema: "app",
                table: "background_job_runs",
                columns: new[] { "jobkey", "idempotencykey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_documents_accountid_ownerentitytype_ownerentityid",
                schema: "app",
                table: "documents",
                columns: new[] { "accountid", "ownerentitytype", "ownerentityid" });

            migrationBuilder.CreateIndex(
                name: "IX_documents_sha256hash",
                schema: "app",
                table: "documents",
                column: "sha256hash");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_accountid_documentnumber",
                schema: "app",
                table: "drivers",
                columns: new[] { "accountid", "documentnumber" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_deliveries_accountid_status_channel",
                schema: "app",
                table: "notification_deliveries",
                columns: new[] { "accountid", "status", "channel" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_rules_accountid_rulekey",
                schema: "app",
                table: "notification_rules",
                columns: new[] { "accountid", "rulekey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_public_link_grants_accountid_resourcetype_resourceid",
                schema: "app",
                table: "public_link_grants",
                columns: new[] { "accountid", "resourcetype", "resourceid" });

            migrationBuilder.CreateIndex(
                name: "IX_public_link_grants_subjecttokenidhash",
                schema: "app",
                table: "public_link_grants",
                column: "subjecttokenidhash",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_devices_accounts_accountid",
                schema: "app",
                table: "devices",
                column: "accountid",
                principalSchema: "app",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transporters_accounts_accountid",
                schema: "app",
                table: "transporters",
                column: "accountid",
                principalSchema: "app",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_devices_accounts_accountid",
                schema: "app",
                table: "devices");

            migrationBuilder.DropForeignKey(
                name: "FK_transporters_accounts_accountid",
                schema: "app",
                table: "transporters");

            migrationBuilder.DropTable(
                name: "account_features",
                schema: "app");

            migrationBuilder.DropTable(
                name: "account_support_grants",
                schema: "app");

            migrationBuilder.DropTable(
                name: "alert_events",
                schema: "app");

            migrationBuilder.DropTable(
                name: "audit_events",
                schema: "app");

            migrationBuilder.DropTable(
                name: "background_job_runs",
                schema: "app");

            migrationBuilder.DropTable(
                name: "documents",
                schema: "app");

            migrationBuilder.DropTable(
                name: "drivers",
                schema: "app");

            migrationBuilder.DropTable(
                name: "notification_deliveries",
                schema: "app");

            migrationBuilder.DropTable(
                name: "notification_rules",
                schema: "app");

            migrationBuilder.DropTable(
                name: "public_link_grants",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "ix_users_accountid_username",
                schema: "app",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_transporters_accountid",
                schema: "app",
                table: "transporters");

            migrationBuilder.DropIndex(
                name: "IX_devices_accountid",
                schema: "app",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "accountid",
                schema: "app",
                table: "transporters");

            migrationBuilder.DropColumn(
                name: "accountid",
                schema: "app",
                table: "devices");

            migrationBuilder.CreateIndex(
                name: "IX_users_accountid",
                schema: "app",
                table: "users",
                column: "accountid");
        }
    }
}

