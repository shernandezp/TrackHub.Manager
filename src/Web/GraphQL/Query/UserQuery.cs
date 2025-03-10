﻿// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
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

using TrackHub.Manager.Application.Users.Queries.Get;
using TrackHub.Manager.Application.Users.Queries.GetByAccount;
using TrackHub.Manager.Application.Users.Queries.GetByGroup;
using TrackHub.Manager.Application.Users.Queries.GetSettings;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<UserVm> GetUser([Service] ISender sender, [AsParameters] GetUserQuery query)
        => await sender.Send(query);

    public async Task<UserSettingsVm> GetUserSettings([Service] ISender sender)
        => await sender.Send(new GetUserSettingsQuery());

    public async Task<IReadOnlyCollection<UserVm>> GetUsersByAccount([Service] ISender sender, [AsParameters] GetUsersByAccountQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<UserVm>> GetUsersByGroup([Service] ISender sender, [AsParameters] GetUsersByGroupQuery query)
        => await sender.Send(query);
}
