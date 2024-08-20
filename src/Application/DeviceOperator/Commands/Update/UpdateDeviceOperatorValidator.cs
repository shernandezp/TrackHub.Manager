namespace TrackHub.Manager.Application.DeviceOperator.Commands.Update;

public sealed class UpdateDeviceOperatorValidator : AbstractValidator<UpdateDeviceOperatorCommand>
{
    public UpdateDeviceOperatorValidator()
    {
        RuleFor(v => v.DeviceOperator)
            .NotEmpty();

        RuleFor(v => v.DeviceOperator.DeviceOperatorId)
            .NotEmpty();

        RuleFor(v => v.DeviceOperator.DeviceId)
            .NotEmpty();

        RuleFor(v => v.DeviceOperator.OperatorId)
            .NotEmpty();

        RuleFor(v => v.DeviceOperator.Identifier)
            .NotEmpty();

        RuleFor(v => v.DeviceOperator.Serial)
            .NotEmpty();
    }
}
