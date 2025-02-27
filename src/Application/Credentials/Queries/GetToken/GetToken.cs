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

using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.CredentialToken.Queries.GetToken;

[Authorize(Resource = Resources.Credentials, Action = Actions.Read)]
public readonly record struct GetTokenQuery(Guid Id) : IRequest<TokenVm>;

// Handles the GetTokenQuery and returns a TokenVm
public class GetTokenQueryHandler(ICredentialReader reader, IConfiguration configuration) : IRequestHandler<GetTokenQuery, TokenVm>
{
    // Handles the GetTokenQuery and returns a TokenVm
    public async Task<TokenVm> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "CredentialToken key not found.");
        return await reader.GetTokenAsync(request.Id, key, cancellationToken);
    }
}
