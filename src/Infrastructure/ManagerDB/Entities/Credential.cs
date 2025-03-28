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

using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.Entities;

public sealed class Credential(string uri, string username, string password, string? key, string? key2, string salt, Guid operatorId) : BaseAuditableEntity
{
    private Operator? _operator;

    public Guid CredentialId { get; set; } = Guid.NewGuid();
    public string Uri { get; set; } = uri;
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
    public string? Key { get; set; } = key;
    public string? Key2 { get; set; } = key2;
    public string Salt { get; set; } = salt;
    public string? Token { get; set; }
    public DateTimeOffset? TokenExpiration { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiration { get; set; }
    public Guid OperatorId { get; set; } = operatorId;

    public Operator Operator
    {
        get => _operator ?? throw new InvalidOperationException("Operator is not loaded");
        set => _operator = value;
    }
}
