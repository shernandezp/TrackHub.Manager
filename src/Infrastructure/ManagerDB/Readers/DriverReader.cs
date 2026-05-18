using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class DriverReader(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), IDriverReader
{
    private static int PageSize(int take) => Math.Clamp(take <= 0 ? 50 : take, 1, 500);
    private static int Offset(int skip) => Math.Max(0, skip);

    public async Task<DriverVm> GetDriverAsync(Guid driverId, CancellationToken cancellationToken)
    {
        var accountId = ResolveAccountScope(null);
        return await Context.Drivers
            .Where(x => x.DriverId == driverId && (!accountId.HasValue || x.AccountId == accountId.Value))
            .Select(x => new DriverVm(x.DriverId, x.AccountId, x.Name, x.Phone, x.DocumentType, x.DocumentNumber, x.Active, x.EmployeeCode, x.LicenseNumber, x.LicenseExpiresAt, x.DefaultTransporterId, x.LastModified))
            .FirstAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DriverVm>> GetDriversByAccountAsync(Guid accountId, int skip, int take, CancellationToken cancellationToken)
        => await Context.Drivers
            .Where(x => x.AccountId == RequireAccountAccess(accountId))
            .OrderBy(x => x.Name).ThenBy(x => x.DriverId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new DriverVm(x.DriverId, x.AccountId, x.Name, x.Phone, x.DocumentType, x.DocumentNumber, x.Active, x.EmployeeCode, x.LicenseNumber, x.LicenseExpiresAt, x.DefaultTransporterId, x.LastModified))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DriverAssignmentVm>> GetDriverAssignmentsAsync(Guid driverId, CancellationToken cancellationToken)
    {
        var accountId = ResolveAccountScope(null);
        var driver = await Context.Drivers
            .Where(x => x.DriverId == driverId && (!accountId.HasValue || x.AccountId == accountId.Value))
            .Select(x => new { x.DriverId, x.AccountId, x.DefaultTransporterId, x.Active })
            .FirstAsync(cancellationToken);

        return driver.DefaultTransporterId.HasValue
            ? [new DriverAssignmentVm(driver.DriverId, driver.AccountId, "Transporter", driver.DefaultTransporterId.Value.ToString(), driver.Active)]
            : [];
    }

    public async Task<bool> ValidateDriverAssignmentAsync(Guid driverId, string resourceType, string resourceId, CancellationToken cancellationToken)
    {
        if (!string.Equals(resourceType, "Transporter", StringComparison.OrdinalIgnoreCase) || !Guid.TryParse(resourceId, out var parsedResourceId))
        {
            return false;
        }

        var accountId = ResolveAccountScope(null);
        return await Context.Drivers.AnyAsync(x =>
            x.DriverId == driverId
            && x.Active
            && (!accountId.HasValue || x.AccountId == accountId.Value)
            && x.DefaultTransporterId == parsedResourceId,
            cancellationToken);
    }
}
