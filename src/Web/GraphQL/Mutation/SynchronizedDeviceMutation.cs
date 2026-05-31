using TrackHub.Manager.Application.GpsIntegration.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<OperatorSyncRunVm> SynchronizeOperatorDevices([Service] ISender sender, SynchronizeOperatorDevicesCommand command)
        => await sender.Send(command);

    public async Task<bool> SetSynchronizedDeviceIgnored([Service] ISender sender, SetSynchronizedDeviceIgnoredCommand command)
    { await sender.Send(command); return true; }
}
