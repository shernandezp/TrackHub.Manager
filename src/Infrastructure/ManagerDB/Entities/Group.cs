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

using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.Entities;

public sealed class Group(string name, string description, bool active, Guid accountId) : BaseAuditableEntity
{
    private Account? _account;

    public long GroupId { get; set; }
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public bool Active { get; set; } = active;
    public Guid AccountId { get; set; } = accountId;
    public ICollection<User> Users { get; } = [];
    public ICollection<Transporter> Transporters { get; } = [];

    public Account Account
    {
        get => _account ?? throw new InvalidOperationException("Account is not loaded");
        set => _account = value;
    }
}
