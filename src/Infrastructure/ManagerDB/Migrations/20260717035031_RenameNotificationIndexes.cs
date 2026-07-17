using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <summary>
    /// Gives the spec-05 indexes explicit names under PostgreSQL's 63-character identifier limit.
    /// The original auto-generated names exceeded it, and existing databases may carry either
    /// spelling depending on how the schema was applied: EF's own truncation ("…re~") or
    /// PostgreSQL's plain truncation ("…rea"). Raw SQL with IF EXISTS covers both.
    /// </summary>
    public partial class RenameNotificationIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER INDEX IF EXISTS app."IX_notification_deliveries_recipientprincipaltype_recipient_re~" RENAME TO ix_notification_deliveries_recipient_readat;
                ALTER INDEX IF EXISTS app."IX_notification_deliveries_recipientprincipaltype_recipient_rea" RENAME TO ix_notification_deliveries_recipient_readat;
                ALTER INDEX IF EXISTS app."IX_notification_deliveries_accountid_status_Created" RENAME TO ix_notification_deliveries_account_status_created;
                ALTER INDEX IF EXISTS app."IX_alert_subscriptions_accountid_principaltype_principalid_eve~" RENAME TO ix_alert_subscriptions_principal_filter_channel;
                ALTER INDEX IF EXISTS app."IX_alert_subscriptions_accountid_principaltype_principalid_even" RENAME TO ix_alert_subscriptions_principal_filter_channel;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER INDEX IF EXISTS app.ix_notification_deliveries_recipient_readat RENAME TO "IX_notification_deliveries_recipientprincipaltype_recipient_re~";
                ALTER INDEX IF EXISTS app.ix_notification_deliveries_account_status_created RENAME TO "IX_notification_deliveries_accountid_status_Created";
                ALTER INDEX IF EXISTS app.ix_alert_subscriptions_principal_filter_channel RENAME TO "IX_alert_subscriptions_accountid_principaltype_principalid_eve~";
                """);
        }
    }
}
