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

namespace TrackHub.Manager.Application.Operators.Queries.Get;

[Authorize(Resource = Resources.Operators, Action = Actions.Read)]
public readonly record struct GetOperatorQuery(Guid Id) : IRequest<OperatorVm>;

public class GetOperatorsQueryHandler(IOperatorReader reader) : IRequestHandler<GetOperatorQuery, OperatorVm>
{
    public async Task<OperatorVm> Handle(GetOperatorQuery request, CancellationToken cancellationToken)
        => await reader.GetOperatorAsync(request.Id, cancellationToken);

}
