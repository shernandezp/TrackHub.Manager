// Copyright (c) 2026 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using TrackHub.Manager.Domain.Constants;
using TrackHub.Manager.Infrastructure;
using TrackHub.Manager.Infrastructure.Entities;

namespace TrackHub.Manager.Web.BackgroundServices;

// Expiration scan (spec 04 §10, AC10): for Active documents crossing the 30/15/7-day thresholds, raise
// exactly one DocumentExpiring per (document, threshold) — idempotency key {documentId}:{threshold} so a
// threshold is never notified twice — and raise DocumentExpired past due. Runs host-internally against
// the DB directly. Skips accounts without the `documents` feature (billing gate, spec 04 §10).
public sealed class DocumentExpirationService(
    IServiceScopeFactory scopeFactory,
    ILogger<DocumentExpirationService> logger) : BackgroundService
{
    private const string JobKey = "document-expiration";
    private static readonly TimeSpan Interval = TimeSpan.FromHours(12);
    private static readonly TimeSpan StartupDelay = TimeSpan.FromMinutes(2);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try { await Task.Delay(StartupDelay, stoppingToken); }
        catch (OperationCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try { await RunOnceAsync(stoppingToken); }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { return; }
            catch (Exception ex) { logger.LogError(ex, "Document expiration cycle failed."); }

            try { await Task.Delay(Interval, stoppingToken); }
            catch (OperationCanceledException) { return; }
        }
    }

    private async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var now = DateTimeOffset.UtcNow;
        var maxThreshold = now.AddDays(DocumentLimits.ExpirationThresholdsDays.Max());

        // Enabled-feature accounts only (security jobs like scan are exempt; this is a billing surface).
        var enabledAccounts = await context.AccountFeatures
            .Where(f => f.FeatureKey == FeatureKeys.Documents && f.Enabled
                && (f.EffectiveFrom == null || f.EffectiveFrom <= now)
                && (f.EffectiveTo == null || f.EffectiveTo >= now))
            .Select(f => f.AccountId)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (enabledAccounts.Count == 0)
        {
            return;
        }

        var candidates = await context.Documents
            .Where(d => enabledAccounts.Contains(d.AccountId)
                && d.Status == DocumentStatuses.Active
                && d.ExpiresAt != null
                && d.ExpiresAt <= maxThreshold)
            .Select(d => new { d.DocumentId, d.AccountId, d.Category, ExpiresAt = d.ExpiresAt!.Value })
            .ToListAsync(cancellationToken);

        var expiringRaised = 0;
        var expiredRaised = 0;

        foreach (var doc in candidates)
        {
            if (doc.ExpiresAt <= now)
            {
                if (await TryRaiseAsync(context, doc.DocumentId, doc.AccountId, doc.Category, "expired", doc.ExpiresAt, now, cancellationToken))
                {
                    await MarkExpiredAsync(context, doc.DocumentId, cancellationToken);
                    expiredRaised++;
                }
                continue;
            }

            // Raise only the nearest crossed threshold (smallest T with daysLeft <= T). Idempotency keeps
            // it exactly-once; picking the nearest band means a document that enters the 7-day window
            // never later back-fires the 15/30-day alerts.
            var daysLeft = (doc.ExpiresAt - now).TotalDays;
            var crossed = DocumentLimits.ExpirationThresholdsDays.Where(t => daysLeft <= t).ToList();
            if (crossed.Count > 0
                && await TryRaiseAsync(context, doc.DocumentId, doc.AccountId, doc.Category, crossed.Min().ToString(), doc.ExpiresAt, now, cancellationToken))
            {
                expiringRaised++;
            }
        }

        logger.LogInformation("Document expiration cycle: {Expiring} expiring alert(s), {Expired} expired alert(s) across {Accounts} account(s).",
            expiringRaised, expiredRaised, enabledAccounts.Count);
    }

    // Idempotency key {documentId}:{threshold} guarantees exactly-once per (document, threshold).
    private static async Task<bool> TryRaiseAsync(ApplicationDbContext context, Guid documentId, Guid accountId, string category, string threshold, DateTimeOffset expiresAt, DateTimeOffset now, CancellationToken cancellationToken)
    {
        var idempotencyKey = $"{documentId:N}:{threshold}";
        var already = await context.BackgroundJobRuns.AnyAsync(
            r => r.JobKey == JobKey && r.IdempotencyKey == idempotencyKey && r.Status == "Succeeded", cancellationToken);
        if (already)
        {
            return false;
        }

        var isExpired = threshold == "expired";
        var eventType = isExpired ? "DocumentExpired" : "DocumentExpiring";
        var dedupKey = $"{eventType}:{documentId:N}:{threshold}";
        context.AlertEvents.Add(new AlertEvent(accountId, eventType, isExpired ? "High" : "Warning", "Documents", "Document", documentId.ToString(), "Open", $$"""{"threshold":"{{threshold}}","category":"{{category}}","expiresAt":"{{expiresAt:O}}"}""", dedupKey));
        context.BackgroundJobRuns.Add(new BackgroundJobRun(JobKey, accountId, documentId.ToString(), idempotencyKey, "Succeeded", 1, now) { CompletedAt = DateTimeOffset.UtcNow });
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static async Task MarkExpiredAsync(ApplicationDbContext context, Guid documentId, CancellationToken cancellationToken)
    {
        var document = await context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId, cancellationToken);
        if (document is not null && document.Status == DocumentStatuses.Active)
        {
            context.Documents.Attach(document);
            document.Status = DocumentStatuses.Expired;
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
