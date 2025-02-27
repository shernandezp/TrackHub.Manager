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

namespace TrackHub.Manager.Application.Device.Queries.Get;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceQuery(Guid Id) : IRequest<DeviceVm>;

public class GetDevicesQueryHandler(IDeviceReader reader) : IRequestHandler<GetDeviceQuery, DeviceVm>
{
    public async Task<DeviceVm> Handle(GetDeviceQuery request, CancellationToken cancellationToken)
        => await reader.GetDeviceAsync(request.Id, cancellationToken);

}
