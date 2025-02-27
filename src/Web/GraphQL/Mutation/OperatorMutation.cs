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

using TrackHub.Manager.Application.Operators.Commands.Create;
using TrackHub.Manager.Application.Operators.Commands.Delete;
using TrackHub.Manager.Application.Operators.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<OperatorVm> CreateOperator([Service] ISender sender, CreateOperatorCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateOperator([Service] ISender sender, Guid id, UpdateOperatorCommand command)
    {
        if (id != command.Operator.OperatorId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteOperator([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteOperatorCommand(id));
        return id;
    }
}
