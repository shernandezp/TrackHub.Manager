namespace TrackHub.Manager.Application.Credentials.Command.Update;

public sealed class UpdateOperatorCredentialValidator : AbstractValidator<UpdateOperatorCredentialCommand>
{
    public UpdateOperatorCredentialValidator()
    {
        RuleFor(v => v.Credential)
            .NotEmpty();

        RuleFor(v => v.Credential.OperatorId)
            .NotEmpty();

        RuleFor(v => v.Credential.Uri)
            .NotEmpty();

    }
}
