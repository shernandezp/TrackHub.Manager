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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    /// <inheritdoc />
    public partial class AddSettingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_settings",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    maps = table.Column<string>(type: "text", nullable: false),
                    storelastposition = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_account_settings_accounts_id",
                        column: x => x.id,
                        principalSchema: "app",
                        principalTable: "accounts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_settings",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    language = table.Column<string>(type: "text", nullable: false),
                    style = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_settings_users_id",
                        column: x => x.id,
                        principalSchema: "app",
                        principalTable: "users",
                        principalColumn: "id");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_settings",
                schema: "app");

            migrationBuilder.DropTable(
                name: "user_settings",
                schema: "app");
        }
    }
}
