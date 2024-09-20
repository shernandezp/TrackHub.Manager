namespace TrackHub.Manager.Application.Accounts.Commands.UpdateSettings;

public sealed class UpdateAccountSettingsValidator : AbstractValidator<UpdateAccountSettingsCommand>
{
    public UpdateAccountSettingsValidator()
    {
        RuleFor(v => v.AccountSettings)
            .NotEmpty();

        RuleFor(v => v.AccountSettings.AccountId)
            .NotEmpty();

        RuleFor(v => v.AccountSettings.Maps)
            .NotEmpty();
    }
}
