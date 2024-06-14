using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface IGroupWriter
{
    Task<GroupVm> CreateGroupAsync(GroupDto groupDto, CancellationToken cancellationToken);
    Task DeleteGroupAsync(Guid groupId, CancellationToken cancellationToken);
    Task UpdateGroupAsync(UpdateGroupDto groupDto, CancellationToken cancellationToken);
}
