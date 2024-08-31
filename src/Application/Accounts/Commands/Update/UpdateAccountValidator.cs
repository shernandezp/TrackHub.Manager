namespace TrackHub.Manager.Application.Accounts.Commands.Update;
public sealed class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountValidator()
    {
        RuleFor(v => v.Account)
            .NotEmpty();

        RuleFor(v => v.Account.AccountId)
            .NotEmpty();

        RuleFor(v => v.Account.Name)
            .NotEmpty();

        RuleFor(v => v.Account.TypeId)
            .NotEmpty();
    }
}
