// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
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

using Common.Domain.Helpers;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// AccountSettingsReader class for reading account settings data
public sealed class AccountSettingsReader(IApplicationDbContext context) : IAccountSettingsReader
{
    /// <summary>
    /// Retrieves an account settings by ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns an AccountSettingsVm object</returns>
    public async Task<AccountSettingsVm> GetAccountSettingsAsync(Guid id, CancellationToken cancellationToken)
        => await context.AccountSettings
            .Where(a => a.AccountId.Equals(id))
            .Select(a => new AccountSettingsVm(
                a.AccountId,
                a.Maps,
                a.MapsKey,
                a.OnlineInterval,
                a.StoreLastPosition,
                a.StoringInterval,
                a.RefreshMap,
                a.RefreshMapInterval))
            .FirstAsync(cancellationToken);

    /// <summary>
    /// Retrieves a collection of account settings
    /// </summary>
    /// <param name="filters">Filters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns a collection of AccountSettingsVm objects</returns>
    public async Task<IReadOnlyCollection<AccountSettingsVm>> GetAccountSettingsAsync(Filters filters, CancellationToken cancellationToken)
    {
        var query = context.AccountSettings.AsQueryable();
        query = filters.Apply(query);

        return await query
            .Select(a => new AccountSettingsVm(
                a.AccountId,
                a.Maps,
                a.MapsKey,
                a.OnlineInterval,
                a.StoreLastPosition,
                a.StoringInterval,
                a.RefreshMap,
                a.RefreshMapInterval))
            .ToListAsync(cancellationToken);
    }
}
