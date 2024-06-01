using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Records;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class CategoryWriter(IApplicationDbContext context) : ICategoryWriter
{
    public async Task<CategoryVm> CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        var category = new Category(
            categoryDto.Name,
            categoryDto.Description,
            categoryDto.Type);

        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CategoryVm(
            category.CategoryId,
            category.Name,
            category.Description,
            category.Type);
    }

    public async Task UpdateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories.FindAsync([categoryDto.CategoryId], cancellationToken)
            ?? throw new NotFoundException(nameof(Category), $"{categoryDto.CategoryId}");

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;
        category.Type = categoryDto.Type;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories.FindAsync([categoryId], cancellationToken)
            ?? throw new NotFoundException(nameof(Category), $"{categoryId}");

        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);
    }
}
