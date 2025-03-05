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

using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// AccountReader class for reading account data
public sealed class AccountReader(IApplicationDbContext context) : IAccountReader
{
    /// <summary>
    /// Retrieves an account by ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns an AccountVm object</returns>
    public async Task<AccountVm> GetAccountAsync(Guid id, CancellationToken cancellationToken)
        => await context.Accounts
            .Where(a => a.AccountId.Equals(id))
            .Select(a => new AccountVm(
                a.AccountId,
                a.Name,
                a.Description,
                (AccountType)a.Type,
                a.Type,
                a.Active,
                a.LastModified))
            .FirstAsync(cancellationToken);

    /// <summary>
    /// Retrieves all accounts
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns a collection of AccountVm objects</returns>
    public async Task<IReadOnlyCollection<AccountVm>> GetAccountsAsync(CancellationToken cancellationToken)
        => await context.Accounts
            .Select(a => new AccountVm(
                a.AccountId,
                a.Name,
                a.Description,
                (AccountType)a.Type,
                a.Type,
                a.Active,
                a.LastModified))
            .ToListAsync(cancellationToken);
}
