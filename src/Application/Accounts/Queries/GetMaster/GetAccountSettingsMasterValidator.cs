namespace TrackHub.Manager.Application.Accounts.Queries.GetMaster;

public class GetAccountSettingsMasterValidator : AbstractValidator<GetAccountSettingsMasterQuery>
{
    public GetAccountSettingsMasterValidator()
    {
        RuleFor(x => x.Filter)
            .NotEmpty();
    }
}
