namespace TrackHub.Manager.Application.Device.Commands.Create;

public sealed class CreateDeviceValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceValidator()
    {
        RuleFor(v => v.Device)
            .NotEmpty();

        RuleFor(v => v.Device.Identifier)
            .NotEmpty();

        RuleFor(v => v.Device.Serial)
            .NotEmpty();

        RuleFor(v => v.Device.DeviceTypeId)
            .NotEmpty();

        RuleFor(v => v.Device.TransporterId)
            .NotEmpty();

        RuleFor(v => v.Device.OperatorId)
            .NotEmpty();
    }
}
