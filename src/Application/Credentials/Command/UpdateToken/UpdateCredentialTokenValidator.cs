namespace TrackHub.Manager.Application.Credentials.Command.UpdateToken;

public sealed class UpdateCredentialTokenValidator : AbstractValidator<UpdateCredentialTokenCommand>
{
    public UpdateCredentialTokenValidator()
    {
        RuleFor(v => v.Credential)
            .NotEmpty();

        RuleFor(v => v.Credential.CredentialId)
            .NotEmpty();

    }
}
