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

namespace TrackHub.Manager.Application.Device.Commands.Delete;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public readonly record struct DeleteDeviceCommand(Guid DeviceId) : IRequest;

public class DeleteDeviceCommandHandler(IDeviceWriter writer) : IRequestHandler<DeleteDeviceCommand>
{

    public async Task Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        => await writer.DeleteDeviceAsync(request.DeviceId, cancellationToken);

}
