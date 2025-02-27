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

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    /// <inheritdoc />
    public partial class FixIntervalNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "storingtimelapse",
                schema: "app",
                table: "account_settings",
                newName: "storinginterval");

            migrationBuilder.RenameColumn(
                name: "refreshmaptimer",
                schema: "app",
                table: "account_settings",
                newName: "refreshmapinterval");

            migrationBuilder.RenameColumn(
                name: "onlinetimelapse",
                schema: "app",
                table: "account_settings",
                newName: "onlineinterval");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "storinginterval",
                schema: "app",
                table: "account_settings",
                newName: "storingtimelapse");

            migrationBuilder.RenameColumn(
                name: "refreshmapinterval",
                schema: "app",
                table: "account_settings",
                newName: "refreshmaptimer");

            migrationBuilder.RenameColumn(
                name: "onlineinterval",
                schema: "app",
                table: "account_settings",
                newName: "onlinetimelapse");
        }
    }
}
