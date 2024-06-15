namespace TrackHub.Manager.Application.Credentials.Command.Delete;

public sealed class DeleteCredentialValidator : AbstractValidator<DeleteCredentialCommand>
{
    public DeleteCredentialValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}
