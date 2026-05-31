using System.Text.Json;
using Common.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.PositionHistory, Action = Actions.Delete, PrincipalTypes = "ServiceClient")]
[RequireFeature(FeatureKeys.GpsPositionHistory, AllowGlobalServiceClient = false)]
public readonly record struct PurgeExpiredPositionHistoryCommand(Guid AccountId, DateTimeOffset Cutoff) : IRequest<int>;

public class PurgeExpiredPositionHistoryCommandHandler(
    ITransporterPositionHistoryWriter writer,
    IAlertEventWriter alertWriter,
    IAuditEventWriter auditWriter,
    ICurrentPrincipal principal,
    ILogger<PurgeExpiredPositionHistoryCommandHandler> logger)
    : IRequestHandler<PurgeExpiredPositionHistoryCommand, int>
{
    public async Task<int> Handle(PurgeExpiredPositionHistoryCommand request, CancellationToken cancellationToken)
    {
        var startedAt = DateTimeOffset.UtcNow;
        int purged;
        try
        {
            purged = await writer.PurgeOlderThanAsync(request.AccountId, request.Cutoff, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Position history purge failed for account {AccountId}.", request.AccountId);
            await alertWriter.RecordAlertEventAsync(new AlertEventDto(
                request.AccountId,
                EventType: "GpsPositionRetentionPurgeFailed",
                Severity: "Error",
                SourceModule: "GpsIntegration",
                ResourceType: "PositionHistory",
                ResourceId: request.AccountId.ToString(),
                Status: "Open",
                PayloadJson: JsonSerializer.Serialize(new { request.Cutoff, Error = ex.GetType().Name, ex.Message }),
                DeduplicationKey: $"gps-purge-failed:{request.AccountId:N}:{startedAt:yyyyMMdd}"),
                cancellationToken);
            throw;
        }

        var actorId = principal.UserId?.ToString() ?? principal.ClientId ?? principal.SubjectId ?? "system";
        await auditWriter.CreateAuditEventAsync(new AuditEventDto(
            request.AccountId,
            ActorType: principal.PrincipalType.ToString(),
            ActorId: actorId,
            Action: "gps.position-history.purge",
            ResourceType: "PositionHistory",
            ResourceId: request.AccountId.ToString(),
            Result: "Succeeded",
            OldValuesJson: null,
            NewValuesJson: JsonSerializer.Serialize(new { request.Cutoff, PurgedCount = purged }),
            Reason: null,
            IpAddress: null,
            UserAgent: null,
            CorrelationId: principal.CorrelationId),
            cancellationToken);

        return purged;
    }
}
