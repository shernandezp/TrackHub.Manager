using TrackHub.Manager.Application.GpsIntegration.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<bool> SetOperatorEnabled([Service] ISender sender, SetOperatorEnabledCommand command)
    { await sender.Send(command); return true; }

    public async Task<bool> RotateOperatorCredential([Service] ISender sender, RotateOperatorCredentialCommand command)
    { await sender.Send(command); return true; }

    public async Task<bool> TriggerOperatorDeviceSync([Service] ISender sender, TriggerOperatorDeviceSyncCommand command)
    { await sender.Send(command); return true; }
}
