﻿using TrackHub.Manager.Application.Device.Queries.Get;
using TrackHub.Manager.Application.Device.Queries.GetByAccount;
using TrackHub.Manager.Application.Device.Queries.GetByOperator;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{

    public async Task<DeviceVm> GetDevice([Service] ISender sender, [AsParameters] GetDeviceQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDeviceByUserByOperator([Service] ISender sender, [AsParameters] GetDeviceByUserByOperatorQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByAccount([Service] ISender sender)
        => await sender.Send(new GetDevicesByAccountQuery());
}
