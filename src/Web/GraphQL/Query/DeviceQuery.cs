using TrackHub.Manager.Application.Devices.Queries.Get;
using TrackHub.Manager.Application.Devices.Queries.GetByAccount;
using TrackHub.Manager.Application.Devices.Queries.GetByGroup;
using TrackHub.Manager.Application.Devices.Queries.GetByUser;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<DeviceVm> GetDevice([Service] ISender sender, [AsParameters] GetDeviceQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByAccount([Service] ISender sender, [AsParameters] GetDeviceByAccountQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByGroup([Service] ISender sender, [AsParameters] GetDeviceByGroupQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByOperatorByGroup([Service] ISender sender, [AsParameters] GetDeviceByGroupByOperatorQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUser([Service] ISender sender, [AsParameters] GetDeviceByUserQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUserByOperator([Service] ISender sender, [AsParameters] GetDeviceByUserByOperatorQuery query)
        => await sender.Send(query);
}
