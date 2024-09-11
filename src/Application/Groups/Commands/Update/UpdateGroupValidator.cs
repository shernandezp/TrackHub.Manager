namespace TrackHub.Manager.Application.Groups.Commands.Update;

public sealed class UpdateGroupValidator : AbstractValidator<UpdateGroupCommand>
{
    public UpdateGroupValidator()
    {
        RuleFor(v => v.Group)
            .NotEmpty();

        RuleFor(v => v.Group.GroupId)
            .NotEmpty();

        RuleFor(v => v.Group.Name)
            .NotEmpty();

        RuleFor(v => v.Group.Description)
            .NotEmpty();

    }
}
