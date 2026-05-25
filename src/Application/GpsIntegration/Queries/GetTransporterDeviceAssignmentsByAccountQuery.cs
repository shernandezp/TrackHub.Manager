namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Read)]

public readonly record struct GetTransporterDeviceAssignmentsByAccountQuery(Guid AccountId, bool ActiveOnly = false)
    : IRequest<IReadOnlyCollection<TransporterDeviceAssignmentVm>>;

public class GetTransporterDeviceAssignmentsByAccountQueryHandler(ITransporterDeviceAssignmentReader reader)
    : IRequestHandler<GetTransporterDeviceAssignmentsByAccountQuery, IReadOnlyCollection<TransporterDeviceAssignmentVm>>
{
    public Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> Handle(GetTransporterDeviceAssignmentsByAccountQuery request, CancellationToken cancellationToken)
        => reader.GetByAccountAsync(request.AccountId, request.ActiveOnly, cancellationToken);
}
