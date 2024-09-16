namespace TrackHub.Manager.Application.Users.Commands.UpdateSettings;

[Authorize(Resource = Resources.Profile, Action = Actions.Edit)]
public readonly record struct UpdateUserSettingsCommand(UserSettingsDto UserSettings) : IRequest;

public class UpdateUserSettingsCommandHandler(IUserSettingsWriter writer) : IRequestHandler<UpdateUserSettingsCommand>
{
    public async Task Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
        => await writer.UpdateUserSettingsAsync(request.UserSettings, cancellationToken);
}
