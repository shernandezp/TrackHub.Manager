using Common.Domain.Enums;
using TrackHub.Manager.Application.Categories.Commands.CreateCategory;
using TrackHub.Manager.Application.Categories.Commands.DeleteCategory;
using TrackHub.Manager.Application.Categories.Commands.UpdateCategory;
using TrackHub.Manager.Application.Categories.Queries.GetCategoriesByType;
using TrackHub.Manager.Application.Categories.Queries.GetCategory;
using TrackHub.Manager.Domain.Models;

namespace TrackHub.Manager.Web.Endpoints;

public class Categories : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetCategory)
            .MapGet(GetCategories, "ByType/{id}")
            .MapPost(CreateCategory)
            .MapPut(UpdateCategory, "{id}")
            .MapDelete(DeleteCategory, "{id}");
    }

    public async Task<CategoryVm> GetCategory(ISender sender, [AsParameters] GetCategoryQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<CategoryVm>> GetCategories(ISender sender, CategoryType id)
        => await sender.Send(new GetCategoriesByTypeQuery(id));

    public async Task<CategoryVm> CreateCategory(ISender sender, CreateCategoryCommand command)
        => await sender.Send(command);

    public async Task<IResult> UpdateCategory(ISender sender, Guid id, UpdateCategoryCommand command)
    {
        if (id != command.Category.CategoryId) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteCategory(ISender sender, Guid id)
    {
        await sender.Send(new DeleteCategoryCommand(id));
        return Results.NoContent();
    }
}
