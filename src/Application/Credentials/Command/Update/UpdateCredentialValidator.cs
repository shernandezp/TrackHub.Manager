namespace TrackHub.Manager.Application.Credentials.Command.Update;

public sealed class UpdateCredentialValidator : AbstractValidator<UpdateCredentialCommand>
{
    public UpdateCredentialValidator()
    {
        RuleFor(v => v.Credential)
            .NotEmpty();

        RuleFor(v => v.Credential.CredentialId)
            .NotEmpty();

        RuleFor(v => v.Credential.Username)
            .NotEmpty();

        RuleFor(v => v.Credential.Password)
            .NotEmpty();

        RuleFor(v => v.Credential.Uri)
            .NotEmpty();

    }
}
