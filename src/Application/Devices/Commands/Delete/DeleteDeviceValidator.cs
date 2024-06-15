namespace TrackHub.Manager.Application.Devices.Commands.Delete;

public sealed class DeleteDeviceValidator : AbstractValidator<DeleteDeviceCommand>
{
    public DeleteDeviceValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}
