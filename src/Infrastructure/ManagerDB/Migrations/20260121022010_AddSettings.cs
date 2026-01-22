using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TrackHub.Manager.Domain.Models;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "accounts",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reports",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transporter_type",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accbased = table.Column<bool>(type: "boolean", nullable: false),
                    stoppedgap = table.Column<double>(type: "double precision", nullable: false),
                    maxdistance = table.Column<double>(type: "double precision", nullable: false),
                    maxtimegap = table.Column<double>(type: "double precision", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transporter_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "account_settings",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    maps = table.Column<string>(type: "text", nullable: false),
                    mapskey = table.Column<string>(type: "text", nullable: true),
                    onlineinterval = table.Column<int>(type: "integer", nullable: false),
                    storelastposition = table.Column<bool>(type: "boolean", nullable: false),
                    storinginterval = table.Column<int>(type: "integer", nullable: false),
                    refreshmap = table.Column<bool>(type: "boolean", nullable: false),
                    refreshmapinterval = table.Column<int>(type: "integer", nullable: false),
                    geofencing = table.Column<bool>(type: "boolean", nullable: false),
                    tripmanagement = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "groups",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_groups_accounts_accountid",
                        column: x => x.accountid,
                        principalSchema: "app",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operators",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    emailaddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    contactname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    protocoltype = table.Column<int>(type: "integer", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operators", x => x.id);
                    table.ForeignKey(
                        name: "FK_operators_accounts_accountid",
                        column: x => x.accountid,
                        principalSchema: "app",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_accounts_accountid",
                        column: x => x.accountid,
                        principalSchema: "app",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transporters",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    transportertypeid = table.Column<short>(type: "smallint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transporters", x => x.id);
                    table.ForeignKey(
                        name: "FK_transporters_transporter_type_transportertypeid",
                        column: x => x.transportertypeid,
                        principalSchema: "app",
                        principalTable: "transporter_type",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "credentials",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    uri = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    key = table.Column<string>(type: "text", nullable: true),
                    key2 = table.Column<string>(type: "text", nullable: true),
                    salt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    token = table.Column<string>(type: "text", nullable: true),
                    tokenexpiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    refreshtoken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    operatorid = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credentials", x => x.id);
                    table.ForeignKey(
                        name: "FK_credentials_operators_operatorid",
                        column: x => x.operatorid,
                        principalSchema: "app",
                        principalTable: "operators",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_group",
                schema: "app",
                columns: table => new
                {
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    groupid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_group", x => new { x.groupid, x.userid });
                    table.ForeignKey(
                        name: "FK_user_group_groups_groupid",
                        column: x => x.groupid,
                        principalSchema: "app",
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_group_users_userid",
                        column: x => x.userid,
                        principalSchema: "app",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_settings",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    language = table.Column<string>(type: "text", nullable: false),
                    style = table.Column<string>(type: "text", nullable: false),
                    navbar = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "devices",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    identifier = table.Column<int>(type: "integer", maxLength: 100, nullable: false),
                    serial = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    devicetypeid = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    transporterid = table.Column<Guid>(type: "uuid", nullable: true),
                    operatorid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.id);
                    table.ForeignKey(
                        name: "FK_devices_operators_operatorid",
                        column: x => x.operatorid,
                        principalSchema: "app",
                        principalTable: "operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_devices_transporters_transporterid",
                        column: x => x.transporterid,
                        principalSchema: "app",
                        principalTable: "transporters",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "transporter_group",
                schema: "app",
                columns: table => new
                {
                    transporterid = table.Column<Guid>(type: "uuid", nullable: false),
                    groupid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transporter_group", x => new { x.groupid, x.transporterid });
                    table.ForeignKey(
                        name: "FK_transporter_group_groups_groupid",
                        column: x => x.groupid,
                        principalSchema: "app",
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transporter_group_transporters_transporterid",
                        column: x => x.transporterid,
                        principalSchema: "app",
                        principalTable: "transporters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transporter_position",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterid = table.Column<Guid>(type: "uuid", nullable: false),
                    geometryid = table.Column<Guid>(type: "uuid", nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    altitude = table.Column<double>(type: "double precision", nullable: true),
                    datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    offset = table.Column<TimeSpan>(type: "interval", nullable: false),
                    speed = table.Column<double>(type: "double precision", nullable: false),
                    course = table.Column<double>(type: "double precision", nullable: true),
                    eventId = table.Column<int>(type: "integer", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    attributes = table.Column<AttributesVm>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transporter_position", x => x.id);
                    table.ForeignKey(
                        name: "FK_transporter_position_transporters_transporterid",
                        column: x => x.transporterid,
                        principalSchema: "app",
                        principalTable: "transporters",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_credentials_operatorid",
                schema: "app",
                table: "credentials",
                column: "operatorid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_devices_operatorid",
                schema: "app",
                table: "devices",
                column: "operatorid");

            migrationBuilder.CreateIndex(
                name: "IX_devices_transporterid",
                schema: "app",
                table: "devices",
                column: "transporterid");

            migrationBuilder.CreateIndex(
                name: "IX_groups_accountid",
                schema: "app",
                table: "groups",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_operators_accountid",
                schema: "app",
                table: "operators",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_transporter_group_transporterid",
                schema: "app",
                table: "transporter_group",
                column: "transporterid");

            migrationBuilder.CreateIndex(
                name: "IX_transporter_position_transporterid",
                schema: "app",
                table: "transporter_position",
                column: "transporterid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transporters_transportertypeid",
                schema: "app",
                table: "transporters",
                column: "transportertypeid");

            migrationBuilder.CreateIndex(
                name: "IX_user_group_userid",
                schema: "app",
                table: "user_group",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_users_accountid",
                schema: "app",
                table: "users",
                column: "accountid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_settings",
                schema: "app");

            migrationBuilder.DropTable(
                name: "credentials",
                schema: "app");

            migrationBuilder.DropTable(
                name: "devices",
                schema: "app");

            migrationBuilder.DropTable(
                name: "reports",
                schema: "app");

            migrationBuilder.DropTable(
                name: "transporter_group",
                schema: "app");

            migrationBuilder.DropTable(
                name: "transporter_position",
                schema: "app");

            migrationBuilder.DropTable(
                name: "user_group",
                schema: "app");

            migrationBuilder.DropTable(
                name: "user_settings",
                schema: "app");

            migrationBuilder.DropTable(
                name: "operators",
                schema: "app");

            migrationBuilder.DropTable(
                name: "transporters",
                schema: "app");

            migrationBuilder.DropTable(
                name: "groups",
                schema: "app");

            migrationBuilder.DropTable(
                name: "users",
                schema: "app");

            migrationBuilder.DropTable(
                name: "transporter_type",
                schema: "app");

            migrationBuilder.DropTable(
                name: "accounts",
                schema: "app");
        }
    }
}
