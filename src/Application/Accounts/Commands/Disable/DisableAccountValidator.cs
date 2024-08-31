namespace TrackHub.Manager.Application.Accounts.Commands.Disable;
public sealed class DisableAccountValidator : AbstractValidator<DisableAccountCommand>
{
    public DisableAccountValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}

