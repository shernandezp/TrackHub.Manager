namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Read)]

public readonly record struct GetTransporterDeviceAssignmentsByTransporterQuery(Guid TransporterId, bool ActiveOnly = false)
    : IRequest<IReadOnlyCollection<TransporterDeviceAssignmentVm>>;

public class GetTransporterDeviceAssignmentsByTransporterQueryHandler(ITransporterDeviceAssignmentReader reader)
    : IRequestHandler<GetTransporterDeviceAssignmentsByTransporterQuery, IReadOnlyCollection<TransporterDeviceAssignmentVm>>
{
    public Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> Handle(GetTransporterDeviceAssignmentsByTransporterQuery request, CancellationToken cancellationToken)
        => reader.GetByTransporterAsync(request.TransporterId, request.ActiveOnly, cancellationToken);
}
