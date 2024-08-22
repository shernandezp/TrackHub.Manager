namespace TrackHub.Manager.Application.Device.Commands.Delete;

public sealed class DeleteDeviceValidator : AbstractValidator<DeleteDeviceCommand>
{
    public DeleteDeviceValidator()
    {

        RuleFor(v => v.DeviceId)
            .NotEmpty();
    }
}
