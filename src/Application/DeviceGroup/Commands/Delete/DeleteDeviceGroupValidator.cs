namespace TrackHub.Manager.Application.DeviceGroup.Commands.Delete;

public sealed class DeleteDeviceGroupValidator : AbstractValidator<DeleteDeviceGroupCommand>
{
    public DeleteDeviceGroupValidator()
    {

        RuleFor(v => v.DeviceId)
            .NotEmpty();

        RuleFor(v => v.GroupId)
            .NotEmpty();
    }
}
