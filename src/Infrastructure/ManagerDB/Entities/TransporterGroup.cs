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


namespace TrackHub.Manager.Infrastructure.Entities;

public sealed class TransporterGroup
{
    private Transporter? _transporter;
    private Group? _group;

    public required Guid TransporterId { get; set; }
    public required long GroupId { get; set; }

    public Transporter Transporter
    {
        get => _transporter ?? throw new InvalidOperationException("Transporter is not loaded");
        set => _transporter = value;
    }
    public Group Group
    {
        get => _group ?? throw new InvalidOperationException("Group is not loaded");
        set => _group = value;
    }
}
