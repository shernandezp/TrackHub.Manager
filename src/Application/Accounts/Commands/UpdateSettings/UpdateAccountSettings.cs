namespace TrackHub.Manager.Application.Accounts.Commands.UpdateSettings;

[Authorize(Resource = Resources.Accounts, Action = Actions.Edit)]
public readonly record struct UpdateAccountSettingsCommand(AccountSettingsDto AccountSettings) : IRequest;

public class UpdateAccountSettingsCommandHandler(IAccountSettingsWriter writer) : IRequestHandler<UpdateAccountSettingsCommand>
{
    // Handles the update account settings command
    public async Task Handle(UpdateAccountSettingsCommand request, CancellationToken cancellationToken)
        => await writer.UpdateAccountSettingsAsync(request.AccountSettings, cancellationToken);
}
