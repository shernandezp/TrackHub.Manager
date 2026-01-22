// Copyright (c) 2026 Sergio Hernandez. All rights reserved.
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

using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Groups.Queries.GetByUser;

[Authorize(Resource = Resources.Groups, Action = Actions.Read)]
public readonly record struct GetGroupByUserQuery() : IRequest<IReadOnlyCollection<GroupVm>>;

public class GetGroupsQueryHandler(IGroupReader reader, IUser user) : IRequestHandler<GetGroupByUserQuery, IReadOnlyCollection<GroupVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<GroupVm>> Handle(GetGroupByUserQuery request, CancellationToken cancellationToken)
        => await reader.GetGroupsByUserAsync(UserId, cancellationToken);

}
