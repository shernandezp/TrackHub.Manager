using TrackHub.Manager.Application.GpsIntegration.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<bool> SetPositionRetentionPolicy([Service] ISender sender, SetPositionRetentionPolicyCommand command)
    { await sender.Send(command); return true; }

    public async Task<int> PurgeExpiredPositionHistory([Service] ISender sender, PurgeExpiredPositionHistoryCommand command)
        => await sender.Send(command);

    public async Task<bool> AppendPositionHistory([Service] ISender sender, AppendPositionHistoryCommand command)
        => await sender.Send(command);
}
