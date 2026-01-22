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

using TrackHub.Manager.Application.Accounts.Queries.Get;
using TrackHub.Manager.Application.Accounts.Queries.GetAll;
using TrackHub.Manager.Application.Accounts.Queries.GetMaster;
using TrackHub.Manager.Application.Accounts.Queries.GetSettings;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<AccountVm> GetAccount([Service] ISender sender, [AsParameters] GetAccountQuery query)
        => await sender.Send(query);

    public async Task<AccountVm> GetAccountByUser([Service] ISender sender)
        => await sender.Send(new GetAccountByUserQuery());

    public async Task<AccountSettingsVm> GetAccountSettings([Service] ISender sender, [AsParameters] GetAccountSettingsQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<AccountSettingsVm>> GetAccountSettingsMaster([Service] ISender sender, [AsParameters] GetAccountSettingsMasterQuery query)
        => await sender.Send(query);

    public async Task<AccountSettingsVm> GetAccountSettingsByUser([Service] ISender sender)
        => await sender.Send(new GetAccountSettingsByUserQuery());

    public async Task<IReadOnlyCollection<AccountVm>> GetAccounts([Service] ISender sender)
        => await sender.Send(new GetAccountsQuery());

}
