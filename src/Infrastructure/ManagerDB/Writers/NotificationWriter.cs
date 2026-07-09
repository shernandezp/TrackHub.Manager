using Common.Application.Exceptions;
using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class NotificationWriter(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), INotificationWriter
{
    public async Task<NotificationRuleVm> CreateNotificationRuleAsync(NotificationRuleDto notificationRule, CancellationToken cancellationToken)
    {
        var entity = new NotificationRule(RequireAccountWriteAccess(notificationRule.AccountId), notificationRule.RuleKey, notificationRule.RuleType, notificationRule.Enabled, notificationRule.TriggerEvent, notificationRule.RecipientSelector, notificationRule.ChannelsJson, notificationRule.ThrottlingJson, notificationRule.ConfigurationJson);
        await Context.NotificationRules.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task UpdateNotificationRuleAsync(Guid notificationRuleId, NotificationRuleDto notificationRule, CancellationToken cancellationToken)
    {
        var entity = await Context.NotificationRules.FirstAsync(x => x.NotificationRuleId == notificationRuleId, cancellationToken);
        RequireAccountWriteAccess(entity.AccountId);
        if (notificationRule.AccountId != entity.AccountId)
        {
            throw new ForbiddenAccessException();
        }

        Context.NotificationRules.Attach(entity);
        entity.RuleKey = notificationRule.RuleKey;
        entity.RuleType = notificationRule.RuleType;
        entity.Enabled = notificationRule.Enabled;
        entity.TriggerEvent = notificationRule.TriggerEvent;
        entity.RecipientSelector = notificationRule.RecipientSelector;
        entity.ChannelsJson = notificationRule.ChannelsJson;
        entity.ThrottlingJson = notificationRule.ThrottlingJson;
        entity.ConfigurationJson = notificationRule.ConfigurationJson;
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DisableNotificationRuleAsync(Guid notificationRuleId, CancellationToken cancellationToken)
    {
        var entity = await Context.NotificationRules.FirstAsync(x => x.NotificationRuleId == notificationRuleId, cancellationToken);
        RequireAccountWriteAccess(entity.AccountId);
        Context.NotificationRules.Attach(entity);
        entity.Enabled = false;
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<NotificationDeliveryVm> CreateNotificationDeliveryAsync(NotificationDeliveryDto notificationDelivery, CancellationToken cancellationToken)
    {
        var entity = new NotificationDelivery(RequireAccountWriteAccess(notificationDelivery.AccountId), notificationDelivery.NotificationRuleId, notificationDelivery.AlertEventId, notificationDelivery.Channel, notificationDelivery.RecipientPrincipalType, notificationDelivery.Recipient, notificationDelivery.Status);
        await Context.NotificationDeliveries.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return new NotificationDeliveryVm(entity.NotificationDeliveryId, entity.AccountId, entity.NotificationRuleId, entity.AlertEventId, entity.Channel, entity.RecipientPrincipalType, entity.Recipient, entity.Status, entity.Attempts, entity.ProviderMessageId, entity.Error, entity.SentAt, entity.ReadAt, entity.LastModified);
    }

    private static NotificationRuleVm ToVm(NotificationRule x) => new(x.NotificationRuleId, x.AccountId, x.RuleKey, x.RuleType, x.Enabled, x.TriggerEvent, x.RecipientSelector, x.ChannelsJson, x.ThrottlingJson, x.ConfigurationJson, x.LastModified);
}
