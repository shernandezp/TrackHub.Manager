namespace TrackHub.Manager.Application.Users.Commands.Update;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(v => v.User)
            .NotEmpty();

        RuleFor(v => v.User.Username)
            .MaximumLength(ColumnMetadata.DefaultUserNameLength)
            .NotEmpty();

        RuleFor(v => v.User.Active)
            .NotEmpty();
    }
}
