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

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class AccountSettings (Guid accountId)
{
    private Account? _account;

    public Guid AccountId { get; set; } = accountId;
    public string Maps { get; set; } = "OSM";
    public string? MapsKey { get; set; } = "";
    public int OnlineInterval { get; set; } = 60;
    public bool StoreLastPosition { get; set; } = false;
    public int StoringInterval { get; set; } = 360;
    public bool RefreshMap { get; set; } = false;
    public int RefreshMapInterval { get; set; } = 60;

    public Account Account
    {
        get => _account ?? throw new InvalidOperationException("Account is not loaded");
        set => _account = value;
    }
}
