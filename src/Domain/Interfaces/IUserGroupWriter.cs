using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface IUserGroupWriter
{
    Task<UserGroupVm> CreateUserGroupAsync(UserGroupDto userGroupDto, CancellationToken cancellationToken);
    Task DeleteUserGroupAsync(Guid userId, long groupId, CancellationToken cancellationToken);
}
