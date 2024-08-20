using TrackHub.Manager.Application.DeviceOperator.Queries.GetByOperator;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<DeviceOperatorVm>> GetDeviceOperatorByUserByOperator([Service] ISender sender, [AsParameters] GetDeviceOperatorByUserByOperatorQuery query)
        => await sender.Send(query);
}
