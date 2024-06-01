using Common.Application.Attributes;
using Common.Domain.Constants;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Models;

namespace TrackHub.Manager.Application.Categories.Queries.GetCategory;

[Authorize(Resource = Resources.MapScreen, Action = Actions.View)]
public readonly record struct GetCategoryQuery(Guid Id) : IRequest<CategoryVm>;

public class GetCategoryQueryHandler(ICategoryReader reader) : IRequestHandler<GetCategoryQuery, CategoryVm>
{
    public async Task<CategoryVm> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        => await reader.GetCategoryAsync(request.Id, cancellationToken);

}
