namespace TrackHub.Manager.Domain.Records;

public readonly record struct NotificationRuleDto(Guid AccountId, string RuleKey, string RuleType, bool Enabled, string TriggerEvent, string RecipientSelector, string ChannelsJson, string? ThrottlingJson, string? ConfigurationJson);

public readonly record struct NotificationDeliveryDto(Guid AccountId, Guid? NotificationRuleId, Guid? AlertEventId, string Channel, string RecipientPrincipalType, string Recipient, string Status);
