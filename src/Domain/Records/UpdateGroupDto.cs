namespace TrackHub.Manager.Domain.Records;
public readonly record struct UpdateGroupDto(
    Guid GroupId,
    string Name,
    string Description,
    bool IsMaster,
    bool Active
    );
