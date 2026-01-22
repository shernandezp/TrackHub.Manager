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

using TrackHub.Manager.Application.Device.Commands.Process;
using TrackHub.Manager.Application.Device.Commands.Delete;
using TrackHub.Manager.Application.Device.Commands.Wipe;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<bool> ProcessDevice([Service] ISender sender, ProcessDeviceCommand command)
        => await sender.Send(command);

    public async Task<Guid> WipeDevices([Service] ISender sender, Guid operatorId)
    {
        await sender.Send(new WipeDevicesCommand(operatorId));
        return operatorId;
    }

    public async Task<Guid> DeleteDevice([Service] ISender sender, Guid deviceId)
    {
        await sender.Send(new DeleteDeviceCommand(deviceId));
        return deviceId;
    }
}
