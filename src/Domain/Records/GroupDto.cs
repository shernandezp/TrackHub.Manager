namespace TrackHub.Manager.Domain.Records;
public readonly record struct GroupDto(
    string Name,
    string Description,
    bool Active
    );
