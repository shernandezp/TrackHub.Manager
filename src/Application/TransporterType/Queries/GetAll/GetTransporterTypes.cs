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


namespace TrackHub.Manager.Application.TransporterType.Queries.GetAll;

[Authorize(Resource = Resources.TransporterType, Action = Actions.Read)]
public readonly record struct GetTransporterTypesQuery() : IRequest<IReadOnlyCollection<TransporterTypeVm>>;

public class GetTransporterTypesQueryHandler(ITransporterTypeReader reader) : IRequestHandler<GetTransporterTypesQuery, IReadOnlyCollection<TransporterTypeVm>>
{
    public async Task<IReadOnlyCollection<TransporterTypeVm>> Handle(GetTransporterTypesQuery request, CancellationToken cancellationToken)
        => await reader.GetTransporterTypesAsync(cancellationToken);

}
