namespace TrackHub.Manager.Application.UserGroup.Commands.Delete;

public sealed class DeleteUserGroupValidator : AbstractValidator<DeleteUserGroupCommand>
{
    public DeleteUserGroupValidator()
    {

        RuleFor(v => v.UserId)
            .NotEmpty();

        RuleFor(v => v.GroupId)
            .NotEmpty();
    }
}
