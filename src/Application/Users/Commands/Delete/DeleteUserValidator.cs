namespace TrackHub.Manager.Application.Users.Commands.Delete;

public sealed class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}
