using TrackHub.Manager.Application.GpsIntegration.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<OperatorHealthVm> GetOperatorHealth([Service] ISender sender, [AsParameters] GetOperatorHealthQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<OperatorHealthCheckVm>> GetOperatorHealthHistory([Service] ISender sender, [AsParameters] GetOperatorHealthHistoryQuery query)
        => await sender.Send(query);

    public async Task<OperatorHealthSummaryVm> GetOperatorHealthSummary([Service] ISender sender, [AsParameters] GetOperatorHealthSummaryQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<OperatorSyncRunVm>> GetOperatorSyncRuns([Service] ISender sender, [AsParameters] GetOperatorSyncRunsQuery query)
        => await sender.Send(query);

    public async Task<GpsIntegrationDashboardVm> GetGpsIntegrationDashboard([Service] ISender sender, [AsParameters] GetGpsIntegrationDashboardQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetSynchronizedDevices([Service] ISender sender, [AsParameters] GetSynchronizedDevicesQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetUnassignedSynchronizedDevices([Service] ISender sender, [AsParameters] GetUnassignedSynchronizedDevicesQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetTransporterDeviceAssignmentsByTransporter([Service] ISender sender, [AsParameters] GetTransporterDeviceAssignmentsByTransporterQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetTransporterDeviceAssignmentsByDevice([Service] ISender sender, [AsParameters] GetTransporterDeviceAssignmentsByDeviceQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetTransporterDeviceAssignmentsByAccount([Service] ISender sender, [AsParameters] GetTransporterDeviceAssignmentsByAccountQuery query)
        => await sender.Send(query);

    public async Task<CredentialMetadataVm?> GetOperatorCredentialMetadata([Service] ISender sender, [AsParameters] GetOperatorCredentialMetadataQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterPositionHistoryVm>> GetPositionHistory([Service] ISender sender, [AsParameters] GetPositionHistoryQuery query)
        => await sender.Send(query);

    public async Task<PositionRetentionPolicyVm> GetPositionRetentionPolicy([Service] ISender sender, [AsParameters] GetPositionRetentionPolicyQuery query)
        => await sender.Send(query);
}
