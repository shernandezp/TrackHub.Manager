using TrackHub.Manager.Application.GpsIntegration.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<int> EmitExpiringCredentialAlerts([Service] ISender sender, EmitExpiringCredentialAlertsCommand command)
        => await sender.Send(command);
}
