using TrackHub.Manager.Application.PointsOfInterest.Queries.GetByAccount;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<PointOfInterestVm>> GetPointsOfInterestByAccount([Service] ISender sender)
        => await sender.Send(new GetPointsOfInterestByAccountQuery());
}
