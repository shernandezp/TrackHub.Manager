using Common.Domain.Constants;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Infrastructure.SecurityApi;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAppSecurityContext(this IServiceCollection services)
    {
        services.AddHeaderPropagation(o => o.Headers.Add("Authorization"));

        services.AddHttpClient(Clients.Security,
            client => client.Timeout = TimeSpan.FromSeconds(30))
            .AddHeaderPropagation();

        services.AddScoped<ISecurityWriter, SecurityWriter>();

        return services;
    }
}
