using Common.Application.Exceptions;
using Common.Application.Interfaces;
using TrackHub.Manager.Domain.Enums;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class TransporterDeviceAssignmentReader(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), ITransporterDeviceAssignmentReader
{
    public async Task<TransporterDeviceAssignmentVm> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await Context.TransporterDeviceAssignments
            .Where(a => a.TransporterDeviceAssignmentId == id)
            .Select(a => new { a.AccountId, Vm = Project(a) })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(TrackHub.Manager.Infrastructure.Entities.TransporterDeviceAssignment), $"{id}");
        RequireAccountAccess(entity.AccountId);
        return entity.Vm;
    }

    public async Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetByTransporterAsync(Guid transporterId, bool activeOnly, CancellationToken cancellationToken)
    {
        var transporter = await Context.Transporters.Where(t => t.TransporterId == transporterId)
            .Select(t => new { t.AccountId }).FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Transporter", $"{transporterId}");
        RequireAccountAccess(transporter.AccountId);
        var q = Context.TransporterDeviceAssignments.Where(a => a.TransporterId == transporterId);
        if (activeOnly) q = q.Where(a => a.Status == (int)AssignmentStatus.Active);
        return await q.OrderByDescending(a => a.EffectiveFrom).Select(a => Project(a)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetByDeviceAsync(Guid deviceId, bool activeOnly, CancellationToken cancellationToken)
    {
        var device = await Context.Devices.Where(d => d.DeviceId == deviceId)
            .Select(d => new { d.Operator!.AccountId }).FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Device", $"{deviceId}");
        RequireAccountAccess(device.AccountId);
        var q = Context.TransporterDeviceAssignments.Where(a => a.DeviceId == deviceId);
        if (activeOnly) q = q.Where(a => a.Status == (int)AssignmentStatus.Active);
        return await q.OrderByDescending(a => a.EffectiveFrom).Select(a => Project(a)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetByAccountAsync(Guid accountId, bool activeOnly, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(accountId);
        var q = Context.TransporterDeviceAssignments.Where(a => a.AccountId == scoped);
        if (activeOnly) q = q.Where(a => a.Status == (int)AssignmentStatus.Active);
        return await q.OrderByDescending(a => a.EffectiveFrom).Select(a => Project(a)).ToListAsync(cancellationToken);
    }

    private static System.Linq.Expressions.Expression<Func<TrackHub.Manager.Infrastructure.Entities.TransporterDeviceAssignment, TransporterDeviceAssignmentVm>> ProjectExpr =>
        a => new TransporterDeviceAssignmentVm(a.TransporterDeviceAssignmentId, a.AccountId, a.TransporterId, a.DeviceId,
            a.EffectiveFrom, a.EffectiveTo, a.Priority, a.IsPrimary, (AssignmentStatus)a.Status, a.AssignmentReason,
            a.CreatedByPrincipalType, a.CreatedByPrincipalId);

    private static TransporterDeviceAssignmentVm Project(TrackHub.Manager.Infrastructure.Entities.TransporterDeviceAssignment a) =>
        new(a.TransporterDeviceAssignmentId, a.AccountId, a.TransporterId, a.DeviceId,
            a.EffectiveFrom, a.EffectiveTo, a.Priority, a.IsPrimary, (AssignmentStatus)a.Status, a.AssignmentReason,
            a.CreatedByPrincipalType, a.CreatedByPrincipalId);
}
