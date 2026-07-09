using Common.Application.Exceptions;
using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class DriverWriter(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), IDriverWriter
{
    public async Task<DriverVm> CreateDriverAsync(DriverDto driver, CancellationToken cancellationToken)
    {
        var entity = new Driver(RequireAccountWriteAccess(driver.AccountId), driver.Name, driver.Phone, driver.DocumentType, driver.DocumentNumber, driver.Active, driver.EmployeeCode, driver.LicenseNumber, driver.LicenseExpiresAt, driver.DefaultTransporterId);
        await Context.Drivers.AddAsync(entity, cancellationToken);
        AddAuditEvent(entity.AccountId, "CreateDriver", "Driver", entity.DriverId.ToString(), null, AuditValues(entity));
        await Context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task UpdateDriverAsync(Guid driverId, DriverDto driver, CancellationToken cancellationToken)
    {
        var entity = await GetDriverForWriteAsync(driverId, cancellationToken);
        if (driver.AccountId != entity.AccountId)
        {
            throw new ForbiddenAccessException();
        }

        Context.Drivers.Attach(entity);
        var oldValues = AuditValues(entity);
        entity.Name = driver.Name;
        entity.Phone = driver.Phone;
        entity.DocumentType = driver.DocumentType;
        entity.DocumentNumber = driver.DocumentNumber;
        entity.Active = driver.Active;
        entity.EmployeeCode = driver.EmployeeCode;
        entity.LicenseNumber = driver.LicenseNumber;
        entity.LicenseExpiresAt = driver.LicenseExpiresAt;
        entity.DefaultTransporterId = driver.DefaultTransporterId;
        AddAuditEvent(entity.AccountId, "UpdateDriver", "Driver", entity.DriverId.ToString(), oldValues, AuditValues(entity));
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeactivateDriverAsync(Guid driverId, CancellationToken cancellationToken)
    {
        var entity = await GetDriverForWriteAsync(driverId, cancellationToken);
        Context.Drivers.Attach(entity);
        var oldValues = AuditValues(entity);
        entity.Active = false;
        AddAuditEvent(entity.AccountId, "DeactivateDriver", "Driver", entity.DriverId.ToString(), oldValues, AuditValues(entity));
        await Context.SaveChangesAsync(cancellationToken);
    }

    private async Task<Driver> GetDriverForWriteAsync(Guid driverId, CancellationToken cancellationToken)
    {
        var entity = await Context.Drivers.FirstAsync(x => x.DriverId == driverId, cancellationToken);
        RequireAccountWriteAccess(entity.AccountId);
        return entity;
    }

    private static DriverVm ToVm(Driver x) => new(x.DriverId, x.AccountId, x.Name, x.Phone, x.DocumentType, x.DocumentNumber, x.Active, x.EmployeeCode, x.LicenseNumber, x.LicenseExpiresAt, x.DefaultTransporterId, x.LastModified);

    private static string AuditValues(Driver driver)
        => $$"""{"name":{{Quote(driver.Name)}},"phone":{{Quote(driver.Phone)}},"documentType":{{Quote(driver.DocumentType)}},"documentNumber":{{Quote(driver.DocumentNumber)}},"active":{{(driver.Active ? "true" : "false")}},"employeeCode":{{Quote(driver.EmployeeCode)}},"licenseNumber":{{Quote(driver.LicenseNumber)}},"defaultTransporterId":{{Quote(driver.DefaultTransporterId?.ToString())}}}""";
}
