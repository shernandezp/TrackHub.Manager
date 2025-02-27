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

using TrackHub.Manager.Application.Groups.Queries.Get;
using TrackHub.Manager.Application.Groups.Queries.GetByAccount;
using TrackHub.Manager.Application.Groups.Queries.GetByUser;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<GroupVm> GetGroup([Service] ISender sender, [AsParameters] GetGroupQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<GroupVm>> GetGroupsByAccount([Service] ISender sender)
        => await sender.Send(new GetGroupByAccountQuery());

    public async Task<IReadOnlyCollection<GroupVm>> GetGroupsByUser([Service] ISender sender)
        => await sender.Send(new GetGroupByUserQuery());
}
