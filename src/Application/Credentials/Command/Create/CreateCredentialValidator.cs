namespace TrackHub.Manager.Application.Credentials.Command.Create;
public sealed class CreateCredentialValidator : AbstractValidator<CreateCredentialCommand>
{
    public CreateCredentialValidator()
    {
        RuleFor(v => v.Credential)
            .NotEmpty();

        RuleFor(v => v.Credential.Username)
            .NotEmpty();

        RuleFor(v => v.Credential.Password)
            .NotEmpty();

        RuleFor(v => v.Credential.OperatorId)
            .NotEmpty();

        RuleFor(v => v.Credential.Uri)
            .NotEmpty();
    }
}
