namespace TrackHub.Manager.Application.Users.Queries.GetByGroup;

[Authorize(Resource = Resources.Users, Action = Actions.Read)]
public readonly record struct GetUsersByGroupQuery(long GroupId) : IRequest<IReadOnlyCollection<UserVm>>;

public class GetUsersQueryHandler(IUserReader reader) : IRequestHandler<GetUsersByGroupQuery, IReadOnlyCollection<UserVm>>
{
    public async Task<IReadOnlyCollection<UserVm>> Handle(GetUsersByGroupQuery request, CancellationToken cancellationToken)
        => await reader.GetUsersByGroupAsync(request.GroupId, cancellationToken);

}
