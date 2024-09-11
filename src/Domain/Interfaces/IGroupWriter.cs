using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface IGroupWriter
{
    Task<GroupVm> CreateGroupAsync(GroupDto groupDto, Guid accountId, CancellationToken cancellationToken);
    Task DeleteGroupAsync(long groupId, CancellationToken cancellationToken);
    Task UpdateGroupAsync(UpdateGroupDto groupDto, CancellationToken cancellationToken);
}
