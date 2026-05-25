namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Read)]

public readonly record struct GetTransporterDeviceAssignmentsByDeviceQuery(Guid DeviceId, bool ActiveOnly = false)
    : IRequest<IReadOnlyCollection<TransporterDeviceAssignmentVm>>;

public class GetTransporterDeviceAssignmentsByDeviceQueryHandler(ITransporterDeviceAssignmentReader reader)
    : IRequestHandler<GetTransporterDeviceAssignmentsByDeviceQuery, IReadOnlyCollection<TransporterDeviceAssignmentVm>>
{
    public Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> Handle(GetTransporterDeviceAssignmentsByDeviceQuery request, CancellationToken cancellationToken)
        => reader.GetByDeviceAsync(request.DeviceId, request.ActiveOnly, cancellationToken);
}
