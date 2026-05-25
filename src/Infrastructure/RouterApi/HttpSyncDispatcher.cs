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

using System.Net.Http.Json;
using Common.Domain.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TrackHub.Manager.Domain.Interfaces;

namespace TrackHub.Manager.Infrastructure.RouterApi;

public sealed class HttpSyncDispatcher(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<HttpSyncDispatcher> logger) : ISyncDispatcher
{
    private readonly string _triggerPath = configuration["Router:SyncTriggerPath"] ?? "/api/Sync/trigger";

    public async Task DispatchManualSyncAsync(Guid accountId, Guid operatorId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(Clients.Router);
        try
        {
            using var response = await client.PostAsJsonAsync(
                _triggerPath,
                new TriggerRequest(accountId, operatorId, "MANUAL", Guid.NewGuid().ToString()),
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                logger.LogWarning(
                    "Router rejected manual sync trigger for operator {OperatorId} (account {AccountId}): {Status} {Body}",
                    operatorId, accountId, (int)response.StatusCode, body);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to dispatch manual sync trigger to Router for operator {OperatorId} (account {AccountId}).",
                operatorId, accountId);
        }
    }

    private sealed record TriggerRequest(Guid AccountId, Guid OperatorId, string TriggerType, string CorrelationId);
}
