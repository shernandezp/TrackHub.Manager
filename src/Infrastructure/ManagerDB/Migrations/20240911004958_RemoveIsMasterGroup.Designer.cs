﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TrackHub.Manager.Infrastructure.ManagerDB;

#nullable disable

namespace TrackHub.Manager.Infrastructure.ManagerDB.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240911004958_RemoveIsMasterGroup")]
    partial class RemoveIsMasterGroup
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Account", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.Property<short>("Type")
                        .HasColumnType("smallint")
                        .HasColumnName("type");

                    b.HasKey("AccountId");

                    b.ToTable("accounts", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Credential", b =>
                {
                    b.Property<Guid>("CredentialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Key2")
                        .HasColumnType("text")
                        .HasColumnName("key2");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("uuid")
                        .HasColumnName("operatorid");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("refreshtoken");

                    b.Property<DateTime?>("RefreshTokenExpiration")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("salt");

                    b.Property<string>("Token")
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.Property<DateTime?>("TokenExpiration")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("tokenexpiration");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("uri");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("username");

                    b.HasKey("CredentialId");

                    b.HasIndex("OperatorId")
                        .IsUnique();

                    b.ToTable("credentials", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Device", b =>
                {
                    b.Property<Guid>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<short>("DeviceTypeId")
                        .HasColumnType("smallint")
                        .HasColumnName("devicetypeid");

                    b.Property<int>("Identifier")
                        .HasMaxLength(100)
                        .HasColumnType("integer")
                        .HasColumnName("identifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("uuid")
                        .HasColumnName("operatorid");

                    b.Property<string>("Serial")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("serial");

                    b.Property<Guid>("TransporterId")
                        .HasColumnType("uuid")
                        .HasColumnName("transporterid");

                    b.HasKey("DeviceId");

                    b.HasIndex("OperatorId");

                    b.HasIndex("TransporterId");

                    b.ToTable("devices", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Group", b =>
                {
                    b.Property<long>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("GroupId"));

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("accountid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.HasKey("GroupId");

                    b.HasIndex("AccountId");

                    b.ToTable("groups", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Operator", b =>
                {
                    b.Property<Guid>("OperatorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("accountid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("address");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("contactname");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("emailaddress");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("phonenumber");

                    b.Property<int>("ProtocolType")
                        .HasColumnType("integer")
                        .HasColumnName("protocoltype");

                    b.HasKey("OperatorId");

                    b.HasIndex("AccountId");

                    b.ToTable("operators", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Transporter", b =>
                {
                    b.Property<Guid>("TransporterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.Property<short>("TransporterTypeId")
                        .HasColumnType("smallint")
                        .HasColumnName("transportertypeid");

                    b.HasKey("TransporterId");

                    b.ToTable("transporters", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.TransporterGroup", b =>
                {
                    b.Property<long>("GroupId")
                        .HasColumnType("bigint")
                        .HasColumnName("groupid");

                    b.Property<Guid>("TransporterId")
                        .HasColumnType("uuid")
                        .HasColumnName("transporterid");

                    b.HasKey("GroupId", "TransporterId");

                    b.HasIndex("TransporterId");

                    b.ToTable("transporter_group", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("accountid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("username");

                    b.HasKey("UserId");

                    b.HasIndex("AccountId");

                    b.ToTable("users", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.UserGroup", b =>
                {
                    b.Property<long>("GroupId")
                        .HasColumnType("bigint")
                        .HasColumnName("groupid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("userid");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("user_group", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Credential", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Operator", "Operator")
                        .WithOne("Credential")
                        .HasForeignKey("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Credential", "OperatorId");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Device", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Operator", "Operator")
                        .WithMany("Devices")
                        .HasForeignKey("OperatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Transporter", "Transporter")
                        .WithMany("Devices")
                        .HasForeignKey("TransporterId");

                    b.Navigation("Operator");

                    b.Navigation("Transporter");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Group", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Account", "Account")
                        .WithMany("Groups")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Operator", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Account", "Account")
                        .WithMany("Operators")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.TransporterGroup", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Transporter", "Transporter")
                        .WithMany()
                        .HasForeignKey("TransporterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Transporter");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.User", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Account", "Account")
                        .WithMany("Users")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.UserGroup", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackHub.Manager.Infrastructure.ManagerDB.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Account", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Operators");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Operator", b =>
                {
                    b.Navigation("Credential");

                    b.Navigation("Devices");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.ManagerDB.Entities.Transporter", b =>
                {
                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
