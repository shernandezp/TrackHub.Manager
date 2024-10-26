using System.Linq.Expressions;

namespace TrackHub.Manager.Domain;

public class Filters(Dictionary<string, object> filters)
{
    private readonly Dictionary<string, object> _filters = filters ?? [];

    public IQueryable<T> Apply<T>(IQueryable<T> query)
    {
        foreach (var filter in _filters)
        {
            query = ApplyFilter(query, filter.Key, filter.Value);
        }
        return query;
    }

    private static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, string propertyName, object value)
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);
        var equal = Expression.Equal(property, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

        return query.Where(lambda);
    }
}
