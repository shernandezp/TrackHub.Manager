namespace TrackHub.Manager.Application.Groups.Commands.Create;

public sealed class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupValidator()
    {
        RuleFor(v => v.Group)
            .NotEmpty();

        RuleFor(v => v.Group.Name)
            .NotEmpty();

        RuleFor(v => v.Group.AccountId)
            .NotEmpty();

        RuleFor(v => v.Group.IsMaster)
            .NotEmpty();

        RuleFor(v => v.Group.Description)
            .NotEmpty();
    }
}
