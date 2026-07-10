using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "capturedatdevicetime",
                schema: "app",
                table: "documents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "capturedlatitude",
                schema: "app",
                table: "documents",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "capturedlongitude",
                schema: "app",
                table: "documents",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "category",
                schema: "app",
                table: "documents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "currentversion",
                schema: "app",
                table: "documents",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "description",
                schema: "app",
                table: "documents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "filename",
                schema: "app",
                table: "documents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "sourcedeviceregistrationid",
                schema: "app",
                table: "documents",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "title",
                schema: "app",
                table: "documents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "document_signatures",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    documentid = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    signerprincipaltype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    signerprincipalid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    signername = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    signatureimagedocumentid = table.Column<Guid>(type: "uuid", nullable: true),
                    legaltextaccepted = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    signedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    createdat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_signatures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "document_types",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    displayname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    required = table.Column<bool>(type: "boolean", nullable: false),
                    expiring = table.Column<bool>(type: "boolean", nullable: false),
                    defaultvaliditydays = table.Column<int>(type: "integer", nullable: true),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    createdat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "document_versions",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    documentid = table.Column<Guid>(type: "uuid", nullable: false),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    versionnumber = table.Column<int>(type: "integer", nullable: false),
                    storageprovider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    storagekey = table.Column<string>(type: "text", nullable: false),
                    sha256hash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sizebytes = table.Column<long>(type: "bigint", nullable: false),
                    contenttype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    filename = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    scanstatus = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    replacedbyprincipaltype = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    replacedbyprincipalid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    createdat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_versions", x => x.id);
                });

            // Backfill existing rows (spec 04 §15): Category = "Other", a derived FileName, CurrentVersion = 1.
            migrationBuilder.Sql(
                "UPDATE app.documents SET category = 'Other' WHERE category IS NULL OR category = '';");
            migrationBuilder.Sql(
                "UPDATE app.documents SET filename = 'document-' || substr(id::text, 1, 8) WHERE filename IS NULL OR filename = '';");
            migrationBuilder.Sql(
                "UPDATE app.documents SET currentversion = 1 WHERE currentversion IS NULL OR currentversion = 0;");

            migrationBuilder.CreateIndex(
                name: "IX_documents_accountid_expiresat_status",
                schema: "app",
                table: "documents",
                columns: new[] { "accountid", "expiresat", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_documents_accountid_ownerentitytype_ownerentityid_category_~",
                schema: "app",
                table: "documents",
                columns: new[] { "accountid", "ownerentitytype", "ownerentityid", "category", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_document_signatures_accountid",
                schema: "app",
                table: "document_signatures",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_document_signatures_documentid",
                schema: "app",
                table: "document_signatures",
                column: "documentid");

            migrationBuilder.CreateIndex(
                name: "IX_document_types_accountid_category",
                schema: "app",
                table: "document_types",
                columns: new[] { "accountid", "category" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_document_versions_accountid",
                schema: "app",
                table: "document_versions",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_document_versions_documentid_versionnumber",
                schema: "app",
                table: "document_versions",
                columns: new[] { "documentid", "versionnumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "document_signatures",
                schema: "app");

            migrationBuilder.DropTable(
                name: "document_types",
                schema: "app");

            migrationBuilder.DropTable(
                name: "document_versions",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_documents_accountid_expiresat_status",
                schema: "app",
                table: "documents");

            migrationBuilder.DropIndex(
                name: "IX_documents_accountid_ownerentitytype_ownerentityid_category_~",
                schema: "app",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "capturedatdevicetime",
                schema: "app",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "capturedlatitude",
                schema: "app",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "capturedlongitude",
                schema: "app",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "category",
                schema: "app",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "currentversion",
                schema: "app",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "description",
                schema: "app",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "filename",
                schema: "app",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "sourcedeviceregistrationid",
                schema: "app",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "title",
                schema: "app",
                table: "documents");
        }
    }
}
