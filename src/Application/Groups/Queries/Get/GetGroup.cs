namespace TrackHub.Manager.Application.Groups.Queries.Get;

[Authorize(Resource = Resources.Groups, Action = Actions.Read)]
public readonly record struct GetGroupQuery(Guid Id) : IRequest<GroupVm>;

public class GetGroupsQueryHandler(IGroupReader reader) : IRequestHandler<GetGroupQuery, GroupVm>
{
    public async Task<GroupVm> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        => await reader.GetGroupAsync(request.Id, cancellationToken);

}
