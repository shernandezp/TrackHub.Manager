using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Accounts.Queries.GetSettings;

[Authorize(Resource = Resources.Accounts, Action = Actions.Read)]
public readonly record struct GetAccountSettingsQuery() : IRequest<AccountSettingsVm>;

public class GetAccountSettingsQueryHandler(IAccountSettingsReader reader, IUserReader userReader, IUser user) : IRequestHandler<GetAccountSettingsQuery, AccountSettingsVm>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    // This method handles the GetAccountSettingsQuery and returns an AccountSettingsVm
    public async Task<AccountSettingsVm> Handle(GetAccountSettingsQuery request, CancellationToken cancellationToken)
    {
        var user = await userReader.GetUserAsync(UserId, cancellationToken);
        return await reader.GetAccountSettingsAsync(user.AccountId, cancellationToken);
    }

}
