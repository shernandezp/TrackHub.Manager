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

using Common.Web.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TrackHub.Manager.Infrastructure;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.ManagerDB;

namespace TrackHub.Manager.Web.Endpoints;

public sealed class PublicLinks : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGet("~/public-links/{publicLinkGrantId:guid}", GetPublicLink);
    }

    public static async Task<IResult> GetPublicLink(
        Guid publicLinkGrantId,
        Guid accountId,
        string resourceType,
        string resourceId,
        string scope,
        string token,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        if (accountId == Guid.Empty
            || string.IsNullOrWhiteSpace(resourceType)
            || string.IsNullOrWhiteSpace(resourceId)
            || string.IsNullOrWhiteSpace(scope)
            || string.IsNullOrWhiteSpace(token))
        {
            return Results.BadRequest();
        }

        var tokenHash = PublicLinkTokenHasher.Hash(token);
        var grant = await context.PublicLinkGrants
            .FirstOrDefaultAsync(x =>
                x.PublicLinkGrantId == publicLinkGrantId
                && x.AccountId == accountId
                && x.ResourceType == resourceType
                && x.ResourceId == resourceId
                && x.SubjectTokenIdHash == tokenHash,
                cancellationToken);

        if (grant == null || grant.RevokedAt.HasValue || !HasScope(grant.Scopes, scope))
        {
            return Results.NotFound();
        }

        if (grant.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            return Results.StatusCode(StatusCodes.Status410Gone);
        }

        grant.AccessCount++;
        grant.LastAccessedAt = DateTimeOffset.UtcNow;
        context.AuditEvents.Add(new AuditEvent(
            grant.AccountId,
            "PublicLink",
            grant.PublicLinkGrantId.ToString(),
            "PublicLinkAccessed",
            "PublicLinkGrant",
            grant.PublicLinkGrantId.ToString(),
            "Succeeded",
            null,
            $$"""{"resourceType":"{{grant.ResourceType}}","resourceId":"{{grant.ResourceId}}","scope":"{{scope}}","accessCount":{{grant.AccessCount}}}""",
            null,
            null,
            null,
            null));
        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok(new
        {
            grant.PublicLinkGrantId,
            grant.AccountId,
            grant.ResourceType,
            grant.ResourceId,
            grant.Scopes,
            grant.Purpose,
            grant.ExpiresAt,
            grant.AccessCount,
            grant.LastAccessedAt
        });
    }

    private static bool HasScope(string scopes, string requestedScope)
        => scopes
            .Split([' ', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Any(scope => string.Equals(scope, requestedScope, StringComparison.OrdinalIgnoreCase));
}
