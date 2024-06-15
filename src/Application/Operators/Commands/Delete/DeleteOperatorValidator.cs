namespace TrackHub.Manager.Application.Operators.Commands.Delete;

public sealed class DeleteOperatorValidator : AbstractValidator<DeleteOperatorCommand>
{
    public DeleteOperatorValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}
