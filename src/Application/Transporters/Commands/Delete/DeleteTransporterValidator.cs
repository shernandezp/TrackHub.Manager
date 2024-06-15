namespace TrackHub.Manager.Application.Transporters.Commands.Delete;

public sealed class DeleteTransporterValidator : AbstractValidator<DeleteTransporterCommand>
{
    public DeleteTransporterValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}
