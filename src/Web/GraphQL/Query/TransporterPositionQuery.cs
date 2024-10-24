using TrackHub.Manager.Application.TransporterPosition.Queries.GetByOperator;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{

    public async Task<IReadOnlyCollection<TransporterPositionVm>> GetTransporterPositionByOperator([Service] ISender sender, [AsParameters] GetTransporterPositionsByOperatorQuery query)
        => await sender.Send(query);

}
