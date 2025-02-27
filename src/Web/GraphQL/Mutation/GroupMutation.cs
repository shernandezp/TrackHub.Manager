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

using TrackHub.Manager.Application.Groups.Commands.Create;
using TrackHub.Manager.Application.Groups.Commands.Delete;
using TrackHub.Manager.Application.Groups.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<GroupVm> CreateGroup([Service] ISender sender, CreateGroupCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateGroup([Service] ISender sender, long id, UpdateGroupCommand command)
    {
        if (id != command.Group.GroupId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<long> DeleteGroup([Service] ISender sender, long id)
    {
        await sender.Send(new DeleteGroupCommand(id));
        return id;
    }
}
