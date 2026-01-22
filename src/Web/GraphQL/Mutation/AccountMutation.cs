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

using TrackHub.Manager.Application.Accounts.Commands.Create;
using TrackHub.Manager.Application.Accounts.Commands.Update;
using TrackHub.Manager.Application.Accounts.Commands.UpdateSettings;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<AccountVm> CreateAccount([Service] ISender sender, CreateAccountCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateAccount([Service] ISender sender, Guid id, UpdateAccountCommand command)
    {
        if (id != command.Account.AccountId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<bool> UpdateAccountSettings([Service] ISender sender, Guid id, UpdateAccountSettingsCommand command)
    {
        if (id != command.AccountSettings.AccountId) return false;
        await sender.Send(command);
        return true;
    }
}
