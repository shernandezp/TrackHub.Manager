namespace TrackHub.Manager.Domain.Records;

public readonly record struct PositionRetentionPolicyDto(bool HistoryEnabled, int RetentionDays, bool LatestOnly);
