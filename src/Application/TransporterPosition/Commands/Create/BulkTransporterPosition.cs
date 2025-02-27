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

namespace TrackHub.Manager.Application.TransporterPosition.Commands.Create;

[Authorize(Resource = Resources.Positions, Action = Actions.Write)]
public readonly record struct BulkTransporterPositionCommand(IEnumerable<TransporterPositionDto> Positions) : IRequest;

public class CreateTransporterCommandHandler(ITransporterPositionWriter writer) : IRequestHandler<BulkTransporterPositionCommand>
{
    public async Task Handle(BulkTransporterPositionCommand request, CancellationToken cancellationToken)
    {
        await writer.BulkTransporterPositionAsync(request.Positions, cancellationToken);
    }
}
