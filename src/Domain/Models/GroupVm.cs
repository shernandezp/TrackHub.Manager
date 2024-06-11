namespace TrackHub.Manager.Domain.Models;
public readonly record struct GroupVm(
    long GroupId,
    string Name,
    string Description,
    bool IsMaster,
    bool Active,
    Guid AccountId
    );
