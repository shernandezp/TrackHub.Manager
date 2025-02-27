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

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;
public sealed class Operator(string name, string? description, string? phoneNumber, string? emailAddress, string? address, string? contactName, int protocolType, Guid accountId) : BaseAuditableEntity
{
    private Account? _account;
    public Guid OperatorId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public string? PhoneNumber { get; set; } = phoneNumber;
    public string? EmailAddress { get; set; } = emailAddress;
    public string? Address { get; set; } = address;
    public string? ContactName { get; set; } = contactName;
    public int ProtocolType { get; set; } = protocolType;
    public Guid AccountId { get; set; } = accountId;
    public Credential? Credential { get; set; }
    public ICollection<Device> Devices { get; } = [];

    public Account Account
    {
        get => _account ?? throw new InvalidOperationException("Account is not loaded");
        set => _account = value;
    }
}
