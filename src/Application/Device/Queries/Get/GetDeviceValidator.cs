namespace TrackHub.Manager.Application.Device.Queries.Get;

public class GetDeviceValidator : AbstractValidator<GetDeviceQuery>
{
    public GetDeviceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
