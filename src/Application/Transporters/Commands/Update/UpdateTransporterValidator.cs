namespace TrackHub.Manager.Application.Transporters.Commands.Update;

public sealed class UpdateTransporterValidator : AbstractValidator<UpdateTransporterCommand>
{
    public UpdateTransporterValidator()
    {
        RuleFor(v => v.Transporter)
            .NotEmpty();

        RuleFor(v => v.Transporter.TransporterId)
            .NotEmpty();

        RuleFor(v => v.Transporter.Name)
            .NotEmpty();

        RuleFor(v => v.Transporter.Icon)
            .NotEmpty();

        RuleFor(v => v.Transporter.TransporterTypeId)
            .NotEmpty();

        RuleFor(v => v.Transporter.DeviceId)
            .NotEmpty();

    }
}
