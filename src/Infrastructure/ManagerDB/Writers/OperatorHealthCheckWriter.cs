using Common.Application.Exceptions;
using Common.Application.Interfaces;
using TrackHub.Manager.Domain.Enums;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class OperatorHealthCheckWriter(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), IOperatorHealthCheckWriter
{
    public async Task<OperatorHealthCheckVm> RecordAsync(OperatorHealthCheckDto dto, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(dto.AccountId);
        var op = await Context.Operators
            .FirstOrDefaultAsync(o => o.OperatorId == dto.OperatorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Operator), dto.OperatorId.ToString());
        if (op.AccountId != scoped)
        {
            throw new ForbiddenAccessException();
        }

        var entity = new OperatorHealthCheck(
            scoped, dto.OperatorId, (int)dto.CheckType, (int)dto.Status, dto.LatencyMs,
            dto.StartedAt, dto.CompletedAt, dto.ErrorCode, dto.ErrorMessage, dto.RetryCount, dto.CorrelationId);

        await Context.OperatorHealthChecks.AddAsync(entity, cancellationToken);

        op.HealthStatus = (int)dto.Status;
        op.LastLatencyMs = dto.LatencyMs;
        if (dto.Status is OperatorHealthStatus.Degraded or OperatorHealthStatus.Offline)
        {
            op.LastFailedSyncAt = dto.CompletedAt ?? dto.StartedAt;
            op.LastFailureCode = dto.ErrorCode;
            op.LastFailureMessage = dto.ErrorMessage;
        }

        await Context.SaveChangesAsync(cancellationToken);
        return new OperatorHealthCheckVm(entity.OperatorHealthCheckId, entity.AccountId, entity.OperatorId,
            (OperatorHealthCheckType)entity.CheckType, (OperatorHealthStatus)entity.Status, entity.LatencyMs,
            entity.StartedAt, entity.CompletedAt, entity.ErrorCode, entity.ErrorMessage, entity.RetryCount, entity.CorrelationId);
    }
}
