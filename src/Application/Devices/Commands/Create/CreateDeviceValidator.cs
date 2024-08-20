namespace TrackHub.Manager.Application.Devices.Commands.Create;

public sealed class CreateDeviceValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceValidator()
    {
        RuleFor(v => v.Device)
            .NotEmpty();

        RuleFor(v => v.Device.Name)
            .NotEmpty();

        RuleFor(v => v.Device.DeviceTypeId)
            .NotEmpty();
    }
}
