namespace TrackHub.Manager.Application.Accounts.Commands.Delete;
public sealed class DeleteAccountValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}

