using TrackHub.Manager.Application.Drivers.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<DriverVm> GetDriver([Service] ISender sender, [AsParameters] GetDriverQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<DriverVm>> GetDriversByAccount([Service] ISender sender, [AsParameters] GetDriversByAccountQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<DriverAssignmentVm>> GetDriverAssignments([Service] ISender sender, [AsParameters] GetDriverAssignmentsQuery query) => await sender.Send(query);
    public async Task<bool> ValidateDriverAssignment([Service] ISender sender, [AsParameters] ValidateDriverAssignmentQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<DriverQualificationVm>> GetDriverQualifications([Service] ISender sender, [AsParameters] GetDriverQualificationsQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<DriverTransporterAssignmentVm>> GetDriverAssignmentHistory([Service] ISender sender, [AsParameters] GetDriverAssignmentHistoryQuery query) => await sender.Send(query);
    public async Task<MyDriverProfileVm> GetMyDriverProfile([Service] ISender sender) => await sender.Send(new GetMyDriverProfileQuery());
}
