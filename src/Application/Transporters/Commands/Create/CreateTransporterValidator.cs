namespace TrackHub.Manager.Application.Transporters.Commands.Create;

public sealed class CreateTransporterValidator : AbstractValidator<CreateTransporterCommand>
{
    public CreateTransporterValidator()
    {
        RuleFor(v => v.Transporter)
            .NotEmpty();

        RuleFor(v => v.Transporter.Name)
            .NotEmpty();

        RuleFor(v => v.Transporter.TransporterTypeId)
            .NotEmpty();
    }
}
