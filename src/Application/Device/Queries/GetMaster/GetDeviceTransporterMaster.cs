using Common.Application.Extensions;
using Common.Application.GraphQL.Inputs;

namespace TrackHub.Manager.Application.Device.Queries.GetMaster;

[Authorize(Resource = Resources.DevicesMaster, Action = Actions.Read)]
public readonly record struct GetDeviceTransporterMasterQuery(FiltersInput Filter) : IRequest<IReadOnlyCollection<DeviceTransporterVm>>;

public class GetDeviceTransporterQueryHandler(IDeviceReader reader) : IRequestHandler<GetDeviceTransporterMasterQuery, IReadOnlyCollection<DeviceTransporterVm>>
{
    public async Task<IReadOnlyCollection<DeviceTransporterVm>> Handle(GetDeviceTransporterMasterQuery request, CancellationToken cancellationToken)
        => await reader.GetDeviceTransportersAsync(request.Filter.GetFilters(), cancellationToken);

}
