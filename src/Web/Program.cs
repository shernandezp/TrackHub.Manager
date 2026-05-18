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

using Ardalis.GuardClauses;
using Common.Application;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TrackHub.Manager.Infrastructure;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Web.GraphQL.Mutation;
using TrackHub.Manager.Web.GraphQL.Query;

var builder = WebApplication.CreateBuilder(args);

var allowedCORSOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string>();
Guard.Against.Null(allowedCORSOrigins, message: $"Allowed Origins configuration for CORS not loaded");

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAppSecurityContext();
builder.Services.AddWebServices();

// Add HealthChecks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddMaxExecutionDepthRule(15)
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment())
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

builder.Services.AddCors(options => options
    .AddPolicy("AllowFrontend",
        builder => builder
                    .WithOrigins(allowedCORSOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));

// Configure HSTS
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365 * 2);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

var app = builder.Build();

app.UseHeaderPropagation();

// Enable CORS
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseExceptionHandler(options => { });

app.MapGet("~/public-links/{publicLinkGrantId:guid}", async (
    Guid publicLinkGrantId,
    Guid accountId,
    string resourceType,
    string resourceId,
    string scope,
    string token,
    ApplicationDbContext context,
    CancellationToken cancellationToken) =>
{
    if (accountId == Guid.Empty
        || string.IsNullOrWhiteSpace(resourceType)
        || string.IsNullOrWhiteSpace(resourceId)
        || string.IsNullOrWhiteSpace(scope)
        || string.IsNullOrWhiteSpace(token))
    {
        return Results.BadRequest();
    }

    var tokenHash = HashPublicLinkToken(token);
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
});

app.MapGraphQL();

app.Run();

static string HashPublicLinkToken(string token)
    => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

static bool HasScope(string scopes, string requestedScope)
    => scopes
        .Split([' ', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Any(scope => string.Equals(scope, requestedScope, StringComparison.OrdinalIgnoreCase));
