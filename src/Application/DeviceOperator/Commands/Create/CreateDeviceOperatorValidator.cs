namespace TrackHub.Manager.Application.DeviceOperator.Commands.Create;

public sealed class CreateDeviceOperatorValidator : AbstractValidator<CreateDeviceOperatorCommand>
{
    public CreateDeviceOperatorValidator()
    {
        RuleFor(v => v.DeviceOperator)
            .NotEmpty();

        RuleFor(v => v.DeviceOperator.Identifier)
            .NotEmpty();

        RuleFor(v => v.DeviceOperator.Serial)
            .NotEmpty();

        RuleFor(v => v.DeviceOperator.DeviceId)
            .NotEmpty();

        RuleFor(v => v.DeviceOperator.OperatorId)
            .NotEmpty();
    }
}
