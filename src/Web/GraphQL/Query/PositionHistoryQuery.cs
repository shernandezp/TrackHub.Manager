using TrackHub.Manager.Application.GpsIntegration.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<TransporterPositionHistoryVm>> GetPositionHistory([Service] ISender sender, [AsParameters] GetPositionHistoryQuery query)
        => await sender.Send(query);

    public async Task<PositionRetentionPolicyVm> GetPositionRetentionPolicy([Service] ISender sender, [AsParameters] GetPositionRetentionPolicyQuery query)
        => await sender.Send(query);
}
