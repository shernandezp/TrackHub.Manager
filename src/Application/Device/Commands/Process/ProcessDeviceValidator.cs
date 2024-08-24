namespace TrackHub.Manager.Application.Device.Commands.Process;

public sealed class ProcessDeviceValidator : AbstractValidator<ProcessDeviceCommand>
{
    public ProcessDeviceValidator()
    {
        RuleFor(v => v.ProcessDevice)
            .NotEmpty();

        RuleFor(v => v.ProcessDevice.Identifier)
            .NotEmpty();

        RuleFor(v => v.ProcessDevice.DeviceTypeId)
            .NotEmpty();

        RuleFor(v => v.ProcessDevice.TransporterTypeId)
            .NotEmpty();

        RuleFor(v => v.ProcessDevice.Serial)
            .NotEmpty();

        RuleFor(v => v.OperatorId)
            .NotEmpty();

    }
}
