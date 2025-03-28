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

using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.Writers;

public sealed class TransporterTypeWriter(IApplicationDbContext context) : ITransporterTypeWriter
{

    /// <summary>
    /// Update a transporter type based on the provided transporter type DTO.
    /// </summary>
    /// <param name="transporterTypeDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateTransporterTypeAsync(TransporterTypeDto transporterTypeDto, CancellationToken cancellationToken)
    {
        var transporterType = await context.TransporterTypes.FindAsync([transporterTypeDto.TransporterTypeId], cancellationToken)
            ?? throw new NotFoundException(nameof(TransporterType), $"{transporterTypeDto.TransporterTypeId}");

        context.TransporterTypes.Attach(transporterType);

        transporterType.AccBased = transporterTypeDto.AccBased;
        transporterType.StoppedGap = transporterTypeDto.StoppedGap;
        transporterType.MaxDistance = transporterTypeDto.MaxDistance;
        transporterType.MaxTimeGap = transporterTypeDto.MaxTimeGap;

        await context.SaveChangesAsync(cancellationToken);
    }
}
