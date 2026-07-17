using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlertsNotificationsDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "payloadjson",
                schema: "app",
                table: "notification_deliveries",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "alert_subscriptions",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    principaltype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    principalid = table.Column<Guid>(type: "uuid", nullable: false),
                    eventtypefilter = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    channel = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    contact = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_subscriptions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_templates",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: true),
                    templatekey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    channel = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    locale = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    body = table.Column<string>(type: "text", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_templates", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notification_deliveries_accountid_status_Created",
                schema: "app",
                table: "notification_deliveries",
                columns: new[] { "accountid", "status", "Created" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_deliveries_recipientprincipaltype_recipient_rea",
                schema: "app",
                table: "notification_deliveries",
                columns: new[] { "recipientprincipaltype", "recipient", "readat" });

            migrationBuilder.CreateIndex(
                name: "IX_alert_subscriptions_accountid_eventtypefilter_enabled",
                schema: "app",
                table: "alert_subscriptions",
                columns: new[] { "accountid", "eventtypefilter", "enabled" });

            migrationBuilder.CreateIndex(
                name: "IX_alert_subscriptions_accountid_principaltype_principalid_even",
                schema: "app",
                table: "alert_subscriptions",
                columns: new[] { "accountid", "principaltype", "principalid", "eventtypefilter", "channel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_templates_accountid_templatekey_channel_locale",
                schema: "app",
                table: "notification_templates",
                columns: new[] { "accountid", "templatekey", "channel", "locale" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert_subscriptions",
                schema: "app");

            migrationBuilder.DropTable(
                name: "notification_templates",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_notification_deliveries_accountid_status_Created",
                schema: "app",
                table: "notification_deliveries");

            migrationBuilder.DropIndex(
                name: "IX_notification_deliveries_recipientprincipaltype_recipient_re~",
                schema: "app",
                table: "notification_deliveries");

            migrationBuilder.DropColumn(
                name: "payloadjson",
                schema: "app",
                table: "notification_deliveries");
        }
    }
}
