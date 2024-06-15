namespace TrackHub.Manager.Application.Groups.Commands.Delete;

public sealed class DeleteGroupValidator : AbstractValidator<DeleteGroupCommand>
{
    public DeleteGroupValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}
