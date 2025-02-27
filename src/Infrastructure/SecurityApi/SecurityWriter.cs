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

using Common.Application.Interfaces;
using Common.Domain.Constants;
using Common.Infrastructure;
using GraphQL;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Infrastructure.SecurityApi;

public class SecurityWriter(IGraphQLClientFactory graphQLClient)
    : GraphQLService(graphQLClient.CreateClient(Clients.Security)), ISecurityWriter
{
    // Creates a new user asynchronously.
    public async Task<UserVm> CreateUserAsync(CreateUserDto user, CancellationToken token)
    {
        var request = new GraphQLRequest
        {
            Query = @"
                    mutation($username: String!, $password: String!, $lastName: String!, $firstName: String!, $emailAddress: String!, $active: Boolean!, $accountId: UUID!)
                    {
                      createManager(
                        command: {
                          user: {
                            username: $username
                            password: $password
                            lastName: $lastName
                            firstName: $firstName
                            emailAddress: $emailAddress
                            active: $active
                          }
                          accountId: $accountId
                        }
                      ) {
                            userId
                            accountId
                            active
                            username
                        }
                    }",
            Variables = new
            {
                user.Username,
                user.Password,
                user.LastName,
                user.FirstName,
                user.EmailAddress,
                user.Active,
                user.AccountId
            }
        };
        var result = await MutationAsync<UserVm>(request, token);
        return result;
    }
}
