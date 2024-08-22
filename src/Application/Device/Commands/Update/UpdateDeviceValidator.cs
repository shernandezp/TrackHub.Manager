namespace TrackHub.Manager.Application.Device.Commands.Update;

public sealed class UpdateDeviceValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceValidator()
    {
        RuleFor(v => v.Device)
            .NotEmpty();

        RuleFor(v => v.Device.DeviceId)
            .NotEmpty();

        RuleFor(v => v.Device.TransporterId)
            .NotEmpty();

        RuleFor(v => v.Device.OperatorId)
            .NotEmpty();

        RuleFor(v => v.Device.Identifier)
            .NotEmpty();

        RuleFor(v => v.Device.Serial)
            .NotEmpty();
    }
}
