namespace TrackHub.Manager.Application.Accounts.Queries.GetSettings;

[Authorize(Resource = Resources.Accounts, Action = Actions.Read)]
public readonly record struct GetAccountSettingsQuery(Guid Id) : IRequest<AccountSettingsVm>;

public class GetAccountSettingsQueryHandler(IAccountSettingsReader reader) : IRequestHandler<GetAccountSettingsQuery, AccountSettingsVm>
{
    public async Task<AccountSettingsVm> Handle(GetAccountSettingsQuery request, CancellationToken cancellationToken)
        => await reader.GetAccountSettingsAsync(request.Id, cancellationToken);
}
