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

using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

/// <summary>
/// Provides methods to read report data from the database.
/// </summary>
public sealed class ReportReader(IApplicationDbContext context) : IReportReader
{
    /// <summary>
    /// Asynchronously retrieves a collection of reports from the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only collection of report view models.</returns>
    public async Task<IReadOnlyCollection<ReportVm>> GetReportsAsync(CancellationToken cancellationToken)
        => await context.Reports
            .Select(r => new ReportVm(
                r.ReportId,
                r.Code,
                r.Description,
                (ReportType)r.Type,
                r.Type,
                r.Active))
            .ToListAsync(cancellationToken);
}
