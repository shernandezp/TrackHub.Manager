namespace TrackHub.Manager.Application.Users.Queries.GetUser;
public readonly record struct GetUserQuery(Guid Id) : IRequest<UserVm>;

public class GetUsersQueryHandler(IUserReader reader) : IRequestHandler<GetUserQuery, UserVm>
{
    public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken)
        => await reader.GetUserAsync(request.Id, cancellationToken);

}
