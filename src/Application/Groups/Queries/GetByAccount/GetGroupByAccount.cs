namespace TrackHub.Manager.Application.Groups.Queries.GetByAccount;

[Authorize(Resource = Resources.Groups, Action = Actions.Read)]
public readonly record struct GetGroupByAccountQuery(Guid AccountId) : IRequest<IReadOnlyCollection<GroupVm>>;

public class GetGroupsQueryHandler(IGroupReader reader) : IRequestHandler<GetGroupByAccountQuery, IReadOnlyCollection<GroupVm>>
{
    public async Task<IReadOnlyCollection<GroupVm>> Handle(GetGroupByAccountQuery request, CancellationToken cancellationToken)
        => await reader.GetGroupsByAccountAsync(request.AccountId, cancellationToken);

}
