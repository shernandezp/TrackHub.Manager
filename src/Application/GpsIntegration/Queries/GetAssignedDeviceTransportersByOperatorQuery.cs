namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Read, PrincipalTypes = "User,ServiceClient")]
[AllowCrossAccount("Read side of the same SyncWorker device-sync loop: one global service identity iterates every account's operators to resolve their assigned device/transporter pairs before fetching positions.")]
public readonly record struct GetAssignedDeviceTransportersByOperatorQuery(Guid AccountId, Guid OperatorId)
    : IRequest<IReadOnlyCollection<DeviceTransporterVm>>;

public class GetAssignedDeviceTransportersByOperatorQueryHandler(IDeviceTransporterReader reader)
    : IRequestHandler<GetAssignedDeviceTransportersByOperatorQuery, IReadOnlyCollection<DeviceTransporterVm>>
{
    public Task<IReadOnlyCollection<DeviceTransporterVm>> Handle(GetAssignedDeviceTransportersByOperatorQuery request, CancellationToken cancellationToken)
        => reader.GetAssignedDeviceTransportersByOperatorAsync(request.AccountId, request.OperatorId, cancellationToken);
}
