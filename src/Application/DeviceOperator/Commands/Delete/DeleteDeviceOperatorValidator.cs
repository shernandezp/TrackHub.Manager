namespace TrackHub.Manager.Application.DeviceOperator.Commands.Delete;

public sealed class DeleteDeviceOperatorValidator : AbstractValidator<DeleteDeviceOperatorCommand>
{
    public DeleteDeviceOperatorValidator()
    {

        RuleFor(v => v.DeviceId)
            .NotEmpty();

        RuleFor(v => v.OperatorId)
            .NotEmpty();
    }
}
