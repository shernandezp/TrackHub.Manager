namespace TrackHub.Manager.Application.Operators.Commands.Create;

public sealed class CreateOperatorValidator : AbstractValidator<CreateOperatorCommand>
{
    public CreateOperatorValidator()
    {
        RuleFor(v => v.Operator)
            .NotEmpty();

        RuleFor(v => v.Operator.Name)
            .NotEmpty();

        RuleFor(v => v.Operator.AccountId)
            .NotEmpty();

        RuleFor(v => v.Operator.ProtocolType)
            .NotEmpty();

    }
}
