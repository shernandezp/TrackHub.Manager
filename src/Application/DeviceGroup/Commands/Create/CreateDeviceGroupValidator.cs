namespace TrackHub.Manager.Application.DeviceGroup.Commands.Create;

public sealed class CreateDeviceGroupValidator : AbstractValidator<CreateDeviceGroupCommand>
{
    public CreateDeviceGroupValidator()
    {
        RuleFor(v => v.DeviceGroup)
            .NotEmpty();

        RuleFor(v => v.DeviceGroup.DeviceId)
            .NotEmpty();

        RuleFor(v => v.DeviceGroup.GroupId)
            .NotEmpty();
    }
}
