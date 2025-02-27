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

namespace TrackHub.Manager.Application.Credentials.Command.Delete;

[Authorize(Resource = Resources.Credentials, Action = Actions.Delete)]
public record DeleteCredentialCommand(Guid Id) : IRequest;

public class DeleteCredentialCommandHandler(ICredentialWriter writer) : IRequestHandler<DeleteCredentialCommand>
{
    public async Task Handle(DeleteCredentialCommand request, CancellationToken cancellationToken)
        => await writer.DeleteCredentialAsync(request.Id, cancellationToken);
}
