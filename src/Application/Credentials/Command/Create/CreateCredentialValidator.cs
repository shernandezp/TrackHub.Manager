﻿namespace TrackHub.Manager.Application.Credentials.Command.Create;

public sealed class CreateCredentialValidator : AbstractValidator<CreateCredentialCommand>
{
    public CreateCredentialValidator()
    {
        RuleFor(v => v.Credential)
            .NotEmpty();

        RuleFor(v => v.Credential.OperatorId)
            .NotEmpty();

        RuleFor(v => v.Credential.Uri)
            .NotEmpty()
            .Must(uri => uri.EndsWith('/'))
            .WithMessage("Credential Uri must end with '/'");
    }
}
