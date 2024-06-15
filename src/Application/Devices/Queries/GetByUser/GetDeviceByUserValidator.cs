namespace TrackHub.Manager.Application.Devices.Queries.GetByUser;

public class GetDeviceByUserValidator : AbstractValidator<GetDeviceByUserQuery>
{
    public GetDeviceByUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
