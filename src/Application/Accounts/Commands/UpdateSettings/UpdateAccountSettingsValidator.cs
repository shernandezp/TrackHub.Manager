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

        RuleFor(v => v.AccountSettings.OnlineTimeLapse)
            .GreaterThanOrEqualTo(5);

        RuleFor(v => v.AccountSettings.StoringTimeLapse)
            .GreaterThanOrEqualTo(60);

        RuleFor(v => v.AccountSettings.RefreshMapTimer)
            .GreaterThanOrEqualTo(60);
    }
}
