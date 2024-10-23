using System.Linq.Expressions;

namespace TrackHub.Manager.Infrastructure;

public static class DbSetExtensions
{
    /// <summary>
    /// Adds or updates an entity in the DbSet. If the entity exists, it updates the entity while excluding specified properties.
    /// It might worth considering different approaches for bulk operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <param name="dbSet">The DbSet to add or update the entity in.</param>
    /// <param name="entity">The entity to add or update.</param>
    /// <param name="keySelector">An expression to select the key of the entity.</param>
    /// <param name="excludeProperties">A collection of property names to exclude from the update.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task AddOrUpdateAsync<TEntity, TKey>(
    this DbSet<TEntity> dbSet,
    TEntity entity,
    Expression<Func<TEntity, TKey>> keySelector,
    IEnumerable<string> excludeProperties,
    CancellationToken cancellationToken = default)
    where TEntity : class
    {
        // Get the key value
        var keyValue = keySelector.Compile().Invoke(entity);

        // Ensure the key value is not null to avoid null comparison issues
        if (keyValue == null)
        {
            throw new ArgumentNullException(nameof(keyValue), "Key value cannot be null.");
        }

        // Create a comparison expression for the query
        var parameter = Expression.Parameter(typeof(TEntity), "e");
        var keySelectorBody = Expression.Invoke(keySelector, parameter);
        var keyComparison = Expression.Equal(keySelectorBody, Expression.Constant(keyValue));

        // Build the final lambda expression
        var predicate = Expression.Lambda<Func<TEntity, bool>>(keyComparison, parameter);

        // Check if the entity exists using the provided key selector
        var existingEntity = await dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        if (existingEntity != null)
        {
            // Attach the existing entity to the context
            dbSet.Attach(existingEntity);

            // Update the existing entity, excluding specified properties
            var entry = dbSet.Entry(existingEntity);

            // Get all properties of the entity
            var properties = entry.Properties.Select(p => p.Metadata.Name).ToList();

            // Determine properties to include in the update
            var propertiesToInclude = properties.Except(excludeProperties).Except(excludeProperties);

            // Manually update each property
            foreach (var property in propertiesToInclude)
            {
                var propertyInfo = typeof(TEntity).GetProperty(property);
                if (propertyInfo != null)
                {
                    entry.Property(property).CurrentValue = propertyInfo.GetValue(entity);
                    entry.Property(property).IsModified = true;
                }
            }
        }
        else
        {
            await dbSet.AddAsync(entity, cancellationToken);
        }
    }
}
