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

using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Device.Queries.GetByAccount;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDevicesByAccountQuery() : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDevicesByAccountQueryHandler(IDeviceReader reader, IUserReader userReader, IUser user) : IRequestHandler<GetDevicesByAccountQuery, IReadOnlyCollection<DeviceVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDevicesByAccountQuery request, CancellationToken cancellationToken)
    {
        var user = await userReader.GetUserAsync(UserId, cancellationToken);
        return await reader.GetDevicesByAccountAsync(user.AccountId, cancellationToken);
    }

}
