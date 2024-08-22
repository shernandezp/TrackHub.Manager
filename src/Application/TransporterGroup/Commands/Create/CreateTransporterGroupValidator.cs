namespace TrackHub.Manager.Application.TransporterGroup.Commands.Create;

public sealed class CreateTransporterGroupValidator : AbstractValidator<CreateTransporterGroupCommand>
{
    public CreateTransporterGroupValidator()
    {
        RuleFor(v => v.TransporterGroup)
            .NotEmpty();

        RuleFor(v => v.TransporterGroup.TransporterId)
            .NotEmpty();

        RuleFor(v => v.TransporterGroup.GroupId)
            .NotEmpty();
    }
}
