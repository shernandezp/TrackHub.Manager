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
    public partial class Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transporter_group_users_UserId",
                schema: "app",
                table: "transporter_group");

            migrationBuilder.DropIndex(
                name: "IX_transporter_group_UserId",
                schema: "app",
                table: "transporter_group");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "transporter_group");

            migrationBuilder.RenameColumn(
                name: "DeviceTypeId",
                schema: "app",
                table: "devices",
                newName: "devicetypeid");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "app",
                table: "devices",
                newName: "description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "devicetypeid",
                schema: "app",
                table: "devices",
                newName: "DeviceTypeId");

            migrationBuilder.RenameColumn(
                name: "description",
                schema: "app",
                table: "devices",
                newName: "Description");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "transporter_group",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_transporter_group_UserId",
                schema: "app",
                table: "transporter_group",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_transporter_group_users_UserId",
                schema: "app",
                table: "transporter_group",
                column: "UserId",
                principalSchema: "app",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
