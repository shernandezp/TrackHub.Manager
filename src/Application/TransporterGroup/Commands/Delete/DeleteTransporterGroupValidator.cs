namespace TrackHub.Manager.Application.TransporterGroup.Commands.Delete;

public sealed class DeleteTransporterGroupValidator : AbstractValidator<DeleteTransporterGroupCommand>
{
    public DeleteTransporterGroupValidator()
    {

        RuleFor(v => v.TransporterId)
            .NotEmpty();

        RuleFor(v => v.GroupId)
            .NotEmpty();
    }
}
