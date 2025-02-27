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

using TrackHub.Manager.Application.Transporters.Queries.Get;
using TrackHub.Manager.Application.Transporters.Queries.GetByAccount;
using TrackHub.Manager.Application.Transporters.Queries.GetByGroup;
using TrackHub.Manager.Application.Transporters.Queries.GetByUser;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<TransporterVm> GetTransporter([Service] ISender sender, [AsParameters] GetTransporterQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByAccount([Service] ISender sender)
        => await sender.Send(new GetTransportersByAccountQuery());

    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByGroup([Service] ISender sender, [AsParameters] GetTransporterByGroupQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByUser([Service] ISender sender)
        => await sender.Send(new GetTransporterByUserQuery());

}
