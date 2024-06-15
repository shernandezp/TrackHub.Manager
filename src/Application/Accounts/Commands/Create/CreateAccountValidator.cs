namespace TrackHub.Manager.Application.Accounts.Commands.Create;
public sealed class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(v => v.Account)
            .NotEmpty();

        RuleFor(v => v.Account.Name)
            .NotEmpty();

        RuleFor(v => v.Account.Type)
            .NotEmpty();
    }
}
