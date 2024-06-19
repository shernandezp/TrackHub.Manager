namespace TrackHub.Manager.Application.UserGroup.Commands.Create;

public sealed class CreateUserGroupValidator : AbstractValidator<CreateUserGroupCommand>
{
    public CreateUserGroupValidator()
    {
        RuleFor(v => v.UserGroup)
            .NotEmpty();

        RuleFor(v => v.UserGroup.UserId)
            .NotEmpty();

        RuleFor(v => v.UserGroup.GroupId)
            .NotEmpty();
    }
}
