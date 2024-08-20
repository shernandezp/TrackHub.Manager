namespace TrackHub.Manager.Application.Devices.Commands.Update;

public sealed class UpdateDeviceValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceValidator()
    {
        RuleFor(v => v.Device)
            .NotEmpty();

        RuleFor(v => v.Device.DeviceId)
            .NotEmpty();

        RuleFor(v => v.Device.Name)
            .NotEmpty();

        RuleFor(v => v.Device.DeviceTypeId)
            .NotEmpty();
    }
}
