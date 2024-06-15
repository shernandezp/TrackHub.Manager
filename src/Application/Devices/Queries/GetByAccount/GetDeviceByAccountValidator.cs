namespace TrackHub.Manager.Application.Devices.Queries.GetByAccount;

public class GetDeviceByAccountValidator : AbstractValidator<GetDeviceByAccountQuery>
{
    public GetDeviceByAccountValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty();
    }
}
