using Common.Domain.Enums;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Models;

namespace TrackHub.Manager.Application.Categories.Queries.GetCategoriesByType;

public record GetCategoriesByTypeQuery(CategoryType Type) : IRequest<IReadOnlyCollection<CategoryVm>>;

public class GetCategoriesByTypeQueryHandler(ICategoryReader reader) : IRequestHandler<GetCategoriesByTypeQuery, IReadOnlyCollection<CategoryVm>>
{
    public async Task<IReadOnlyCollection<CategoryVm>> Handle(GetCategoriesByTypeQuery request, CancellationToken cancellationToken)
        => await reader.GetCategoryByTypeAsync(request.Type, cancellationToken);

}
