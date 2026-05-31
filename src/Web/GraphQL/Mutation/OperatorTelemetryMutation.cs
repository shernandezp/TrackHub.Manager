using TrackHub.Manager.Application.GpsIntegration.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<OperatorHealthCheckVm> RecordOperatorHealth([Service] ISender sender, RecordOperatorHealthCommand command)
        => await sender.Send(command);

    public async Task<OperatorSyncRunVm> RecordOperatorSyncRun([Service] ISender sender, RecordOperatorSyncRunCommand command)
        => await sender.Send(command);
}
