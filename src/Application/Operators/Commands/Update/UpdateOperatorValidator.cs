namespace TrackHub.Manager.Application.Operators.Commands.Update;

public sealed class UpdateOperatorValidator : AbstractValidator<UpdateOperatorCommand>
{
    public UpdateOperatorValidator()
    {
        RuleFor(v => v.Operator)
            .NotEmpty();

        RuleFor(v => v.Operator.OperatorId)
            .NotEmpty();

        RuleFor(v => v.Operator.Name)
            .NotEmpty();

        RuleFor(v => v.Operator.ProtocolTypeId)
            .NotEmpty();
    }
}
