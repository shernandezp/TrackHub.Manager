namespace TrackHub.Manager.Application.Device.Queries.GetMaster;

public class GetDeviceTransporterMasterValidator : AbstractValidator<GetDeviceTransporterMasterQuery>
{
    public GetDeviceTransporterMasterValidator()
    {
        RuleFor(x => x.Filter)
            .NotEmpty();
    }
}
