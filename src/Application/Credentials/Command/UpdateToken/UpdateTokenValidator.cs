namespace TrackHub.Manager.Application.Credentials.Command.UpdateToken;

public sealed class UpdateTokenValidator : AbstractValidator<UpdateTokenCommand>
{
    public UpdateTokenValidator()
    {
        RuleFor(v => v.Credential)
            .NotEmpty();

        RuleFor(v => v.Credential.CredentialId)
            .NotEmpty();

    }
}
