using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using TrackHub.Manager.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddHeaderPropagation(o => o.Headers.Add("Authorization"));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IAccountWriter, AccountWriter>();
        services.AddScoped<IAccountReader, AccountReader>();
        services.AddScoped<ICredentialWriter, CredentialWriter>();
        services.AddScoped<ICredentialReader, CredentialReader>();
        services.AddScoped<IDeviceWriter, DeviceWriter>();
        services.AddScoped<IDeviceGroupWriter, DeviceGroupWriter>();
        services.AddScoped<IDeviceOperatorWriter, DeviceOperatorWriter>();
        services.AddScoped<IDeviceOperatorReader, DeviceOperatorReader>();
        services.AddScoped<IDeviceReader, DeviceReader>();
        services.AddScoped<IGroupWriter, GroupWriter>();
        services.AddScoped<IGroupReader, GroupReader>();
        services.AddScoped<IOperatorWriter, OperatorWriter>();
        services.AddScoped<IOperatorReader, OperatorReader>();
        services.AddScoped<ITransporterWriter, TransporterWriter>();
        services.AddScoped<ITransporterReader, TransporterReader>();
        services.AddScoped<IUserWriter, UserWriter>();
        services.AddScoped<IUserReader, UserReader>();
        services.AddScoped<IUserGroupWriter, UserGroupWriter>();

        return services;
    }
}
