using TrackHub.Manager.Application.Categories.Commands.CreateCategory;
using TrackHub.Manager.Application.Categories.Queries.GetCategoriesByType;
using TrackHub.Manager.Application.Categories.Queries.GetCategory;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<CategoryVm> GetCategory([Service] ISender sender, [AsParameters] GetCategoryQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<CategoryVm>> GetCategories([Service] ISender sender, [AsParameters] GetCategoriesByTypeQuery query)
        => await sender.Send(query);

    public async Task<CategoryVm> CreateCategory([Service] ISender sender, CreateCategoryCommand command)
        => await sender.Send(command);

}
