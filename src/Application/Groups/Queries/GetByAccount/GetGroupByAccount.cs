using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Groups.Queries.GetByAccount;

[Authorize(Resource = Resources.Groups, Action = Actions.Read)]
public readonly record struct GetGroupByAccountQuery() : IRequest<IReadOnlyCollection<GroupVm>>;

public class GetGroupsQueryHandler(IGroupReader reader, IUserReader userReader, IUser user) : IRequestHandler<GetGroupByAccountQuery, IReadOnlyCollection<GroupVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<GroupVm>> Handle(GetGroupByAccountQuery request, CancellationToken cancellationToken)
    {
        var user = await userReader.GetUserAsync(UserId, cancellationToken);
        return await reader.GetGroupsByAccountAsync(user.AccountId, cancellationToken); 
    }

}
