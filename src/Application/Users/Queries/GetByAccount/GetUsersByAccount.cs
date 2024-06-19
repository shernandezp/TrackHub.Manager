namespace TrackHub.Manager.Application.Users.Queries.GetByAccount;

[Authorize(Resource = Resources.Users, Action = Actions.Read)]
public readonly record struct GetUsersByAccountQuery(Guid AccountId) : IRequest<IReadOnlyCollection<UserVm>>;

public class GetUsersQueryHandler(IUserReader reader) : IRequestHandler<GetUsersByAccountQuery, IReadOnlyCollection<UserVm>>
{
    public async Task<IReadOnlyCollection<UserVm>> Handle(GetUsersByAccountQuery request, CancellationToken cancellationToken)
        => await reader.GetUsersByAccountAsync(request.AccountId, cancellationToken);

}
