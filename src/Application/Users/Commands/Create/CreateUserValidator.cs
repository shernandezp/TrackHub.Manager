namespace TrackHub.Manager.Application.Users.Commands.Create;
public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(v => v.User)
            .NotEmpty();

        RuleFor(v => v.User.Username)
            .MaximumLength(ColumnMetadata.DefaultUserNameLength)
            .NotEmpty();

        RuleFor(v => v.User.UserId)
            .NotEmpty();

        RuleFor(v => v.User.AccountId)
            .NotEmpty();
    }
}
