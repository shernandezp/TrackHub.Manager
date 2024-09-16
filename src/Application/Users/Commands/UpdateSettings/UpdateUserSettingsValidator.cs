namespace TrackHub.Manager.Application.Users.Commands.UpdateSettings;

public sealed class UpdateUserSettingsValidator : AbstractValidator<UpdateUserSettingsCommand>
{
    public UpdateUserSettingsValidator()
    {
        RuleFor(v => v.UserSettings)
            .NotEmpty();

        RuleFor(v => v.UserSettings.UserId)
            .NotEmpty();

        RuleFor(v => v.UserSettings.Style)
            .NotEmpty();

        RuleFor(v => v.UserSettings.Language)
            .NotEmpty();
    }
}
