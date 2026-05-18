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
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// AccountSettingsWriter class is responsible for writing account settings-related data to the database
public sealed class AccountSettingsWriter(IApplicationDbContext context) : IAccountSettingsWriter
{
    /// <summary>
    /// Creates a new account setting asynchronously
    /// </summary>
    /// <param name="accountSettingsDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created account settings view model</returns>
    public async Task<AccountSettingsVm> CreateAccountSettingsAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var accountSettings = new AccountSettings(accountId);

        await context.AccountSettings.AddAsync(accountSettings, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new AccountSettingsVm(
            accountSettings.AccountId,
            accountSettings.Maps,
            accountSettings.MapsKey,
            accountSettings.OnlineInterval,
            accountSettings.StoreLastPosition,
            accountSettings.StoringInterval,
            accountSettings.RefreshMap,
            accountSettings.RefreshMapInterval,
            accountSettings.EnableGeofencing,
            accountSettings.EnableTripManagement);
    }

    /// <summary>
    /// Updates an existing account setting asynchronously
    /// </summary>
    /// <param name="accountSettingsDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateAccountSettingsAsync(AccountSettingsDto accountSettingsDto, CancellationToken cancellationToken)
    {
        var accountSettings = await context.AccountSettings.FindAsync([accountSettingsDto.AccountId, cancellationToken], cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(AccountSettings), $"{accountSettingsDto.AccountId}");

        context.AccountSettings.Attach(accountSettings);

        accountSettings.Maps = accountSettingsDto.Maps;
        accountSettings.MapsKey = accountSettingsDto.MapsKey;
        accountSettings.OnlineInterval = accountSettingsDto.OnlineInterval;
        accountSettings.StoreLastPosition = accountSettingsDto.StoreLastPosition;
        accountSettings.StoringInterval = accountSettingsDto.StoringInterval;
        accountSettings.RefreshMap = accountSettingsDto.RefreshMap;
        accountSettings.RefreshMapInterval = accountSettingsDto.RefreshMapInterval;
        accountSettings.EnableGeofencing = accountSettingsDto.EnableGeofencing;
        accountSettings.EnableTripManagement = accountSettingsDto.EnableTripManagement;

        await SetAccountFeatureAsync(accountSettings.AccountId, FeatureKeys.Geofencing, accountSettingsDto.EnableGeofencing, cancellationToken);
        await SetAccountFeatureAsync(accountSettings.AccountId, FeatureKeys.TripManagement, accountSettingsDto.EnableTripManagement, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SetAccountFeatureAsync(Guid accountId, string featureKey, bool enabled, CancellationToken cancellationToken)
    {
        var feature = await context.AccountFeatures.FirstOrDefaultAsync(x => x.AccountId == accountId && x.FeatureKey == featureKey, cancellationToken);
        if (feature == null)
        {
            await context.AccountFeatures.AddAsync(new AccountFeature(accountId, featureKey, enabled, "default", "account-settings", null, null, null), cancellationToken);
            return;
        }

        context.AccountFeatures.Attach(feature);
        feature.Enabled = enabled;
        feature.Source = "account-settings";
    }
}
