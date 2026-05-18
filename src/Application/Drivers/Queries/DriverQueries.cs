namespace TrackHub.Manager.Application.Drivers.Queries;

[Authorize(Resource = Resources.Drivers, Action = Actions.Read)]
public readonly record struct GetDriverQuery(Guid DriverId) : IRequest<DriverVm>;
public class GetDriverQueryHandler(IDriverReader reader) : IRequestHandler<GetDriverQuery, DriverVm>
{
    public async Task<DriverVm> Handle(GetDriverQuery request, CancellationToken cancellationToken) => await reader.GetDriverAsync(request.DriverId, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Read)]
public readonly record struct GetDriversByAccountQuery(Guid AccountId, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<DriverVm>>;
public class GetDriversByAccountQueryHandler(IDriverReader reader) : IRequestHandler<GetDriversByAccountQuery, IReadOnlyCollection<DriverVm>>
{
    public async Task<IReadOnlyCollection<DriverVm>> Handle(GetDriversByAccountQuery request, CancellationToken cancellationToken) => await reader.GetDriversByAccountAsync(request.AccountId, request.Skip, request.Take, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Read)]
public readonly record struct GetDriverAssignmentsQuery(Guid DriverId) : IRequest<IReadOnlyCollection<DriverAssignmentVm>>;
public class GetDriverAssignmentsQueryHandler(IDriverReader reader) : IRequestHandler<GetDriverAssignmentsQuery, IReadOnlyCollection<DriverAssignmentVm>>
{
    public async Task<IReadOnlyCollection<DriverAssignmentVm>> Handle(GetDriverAssignmentsQuery request, CancellationToken cancellationToken) => await reader.GetDriverAssignmentsAsync(request.DriverId, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Read)]
public readonly record struct ValidateDriverAssignmentQuery(Guid DriverId, string ResourceType, string ResourceId) : IRequest<bool>;
public class ValidateDriverAssignmentQueryHandler(IDriverReader reader) : IRequestHandler<ValidateDriverAssignmentQuery, bool>
{
    public async Task<bool> Handle(ValidateDriverAssignmentQuery request, CancellationToken cancellationToken) => await reader.ValidateDriverAssignmentAsync(request.DriverId, request.ResourceType, request.ResourceId, cancellationToken);
}
