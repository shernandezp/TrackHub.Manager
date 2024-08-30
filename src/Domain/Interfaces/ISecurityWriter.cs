using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface ISecurityWriter
{
    Task<UserVm> CreateUserAsync(CreateUserDto user, CancellationToken token);
}
