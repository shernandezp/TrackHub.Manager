namespace TrackHub.Manager.Domain.Models;

public readonly record struct NotificationRuleVm(Guid NotificationRuleId, Guid AccountId, string RuleKey, string RuleType, bool Enabled, string TriggerEvent, string RecipientSelector, string ChannelsJson, string? ThrottlingJson, string? ConfigurationJson, DateTimeOffset LastModified);

public readonly record struct NotificationDeliveryVm(Guid NotificationDeliveryId, Guid AccountId, Guid? NotificationRuleId, Guid? AlertEventId, string Channel, string RecipientPrincipalType, string Recipient, string Status, int Attempts, string? ProviderMessageId, string? Error, DateTimeOffset? SentAt, DateTimeOffset? ReadAt, DateTimeOffset LastModified);
