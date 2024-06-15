namespace TrackHub.Manager.Application.Devices.Queries.Get;

public class GetDeviceValidator : AbstractValidator<GetDeviceQuery>
{
    public GetDeviceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
