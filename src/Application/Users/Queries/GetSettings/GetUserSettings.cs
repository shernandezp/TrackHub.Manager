using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Users.Queries.GetSettings;

[Authorize(Resource = Resources.Profile, Action = Actions.Read)]
public readonly record struct GetUserSettingsQuery() : IRequest<UserSettingsVm>;

public class GetUserSettingsQueryHandler(IUserSettingsReader reader, IUser user) : IRequestHandler<GetUserSettingsQuery, UserSettingsVm>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    // This method handles the GetUserSettingsQuery and returns an UserSettingsVm
    public async Task<UserSettingsVm> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
    {
        return await reader.GetUserSettingsAsync(UserId, cancellationToken);
    }

}
