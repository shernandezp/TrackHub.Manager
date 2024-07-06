using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Groups.Queries.GetByUser;

[Authorize(Resource = Resources.Groups, Action = Actions.Read)]
public readonly record struct GetGroupByUserQuery() : IRequest<IReadOnlyCollection<GroupVm>>;

public class GetGroupsQueryHandler(IGroupReader reader, IUser user) : IRequestHandler<GetGroupByUserQuery, IReadOnlyCollection<GroupVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<GroupVm>> Handle(GetGroupByUserQuery request, CancellationToken cancellationToken)
        => await reader.GetGroupsByUserAsync(UserId, cancellationToken);

}
