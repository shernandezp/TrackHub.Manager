using Common.Domain.Enums;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.Readers;

public sealed class CategoryReader(IApplicationDbContext context) : ICategoryReader
{
    public async Task<CategoryVm> GetCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .AsNoTracking()
            .Where(c => c.CategoryId.Equals(id))
            .Select(c => new CategoryVm(
                c.CategoryId,
                c.Name,
                c.Description,
                c.Type))
            .FirstAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CategoryVm>> GetCategoryByTypeAsync(CategoryType type, CancellationToken cancellationToken)
    {
        return await context.Categories
            .AsNoTracking()
            .Where(c => c.Type == type)
            .Select(c => new CategoryVm(
                c.CategoryId,
                c.Name,
                c.Description,
                c.Type))
            .ToListAsync(cancellationToken);
    }
}
