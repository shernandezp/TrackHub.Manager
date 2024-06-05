namespace TrackHub.Manager.Domain.Models;
public record struct UserVm(
    Guid UserId,
    string Username,
    bool Active,
    Guid AccountId);
