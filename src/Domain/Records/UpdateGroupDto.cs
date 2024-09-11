namespace TrackHub.Manager.Domain.Records;
public readonly record struct UpdateGroupDto(
    long GroupId,
    string Name,
    string Description,
    bool Active
    );
