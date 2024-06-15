namespace TrackHub.Manager.Application.Groups.Queries.GetByUser;

[Authorize(Resource = Resources.Groups, Action = Actions.Read)]
public readonly record struct GetGroupByUserQuery(Guid UserId) : IRequest<IReadOnlyCollection<GroupVm>>;

public class GetGroupsQueryHandler(IGroupReader reader) : IRequestHandler<GetGroupByUserQuery, IReadOnlyCollection<GroupVm>>
{
    public async Task<IReadOnlyCollection<GroupVm>> Handle(GetGroupByUserQuery request, CancellationToken cancellationToken)
        => await reader.GetGroupsByUserAsync(request.UserId, cancellationToken);

}
