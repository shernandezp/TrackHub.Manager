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
    public partial class AddMinorDeviceUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                schema: "app",
                table: "transporters");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "app",
                table: "devices",
                newName: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                schema: "app",
                table: "devices",
                newName: "Name");

            migrationBuilder.AddColumn<short>(
                name: "icon",
                schema: "app",
                table: "transporters",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
