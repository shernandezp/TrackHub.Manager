using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface IUserWriter
{
    Task<UserVm> CreateUserAsync(UserDto userDto, CancellationToken cancellationToken);
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateUserAsync(UserDto userDto, CancellationToken cancellationToken);
}
