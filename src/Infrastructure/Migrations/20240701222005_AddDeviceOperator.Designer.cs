﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TrackHub.Manager.Infrastructure;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240701222005_AddDeviceOperator")]
    partial class AddDeviceOperator
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Account", b =>
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

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Credential", b =>
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
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Key2")
                        .IsRequired()
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
                        .IsRequired()
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
                        .IsRequired()
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

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Device", b =>
                {
                    b.Property<Guid>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<short>("DeviceTypeId")
                        .HasColumnType("smallint")
                        .HasColumnName("devicetypeid");

                    b.Property<int>("Identifier")
                        .HasMaxLength(100)
                        .HasColumnType("integer")
                        .HasColumnName("identifier");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.Property<string>("Serial")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("serial");

                    b.HasKey("DeviceId");

                    b.ToTable("devices", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.DeviceGroup", b =>
                {
                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uuid")
                        .HasColumnName("deviceid");

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint")
                        .HasColumnName("groupid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("DeviceId", "GroupId");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("device_group", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.DeviceOperator", b =>
                {
                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uuid")
                        .HasColumnName("deviceid");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("uuid")
                        .HasColumnName("operatorid");

                    b.HasKey("DeviceId", "OperatorId");

                    b.HasIndex("OperatorId");

                    b.ToTable("device_operator", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Group", b =>
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

                    b.Property<bool>("IsMaster")
                        .HasColumnType("boolean")
                        .HasColumnName("ismaster");

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

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Operator", b =>
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

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Transporter", b =>
                {
                    b.Property<Guid>("TransporterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uuid")
                        .HasColumnName("deviceid");

                    b.Property<short>("Icon")
                        .HasColumnType("smallint")
                        .HasColumnName("icon");

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

                    b.HasIndex("DeviceId")
                        .IsUnique();

                    b.ToTable("transporters", "app");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.User", b =>
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

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.UserGroup", b =>
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

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Credential", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Operator", "Operator")
                        .WithOne("Credential")
                        .HasForeignKey("TrackHub.Manager.Infrastructure.Entities.Credential", "OperatorId");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.DeviceGroup", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.DeviceOperator", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Operator", "Operator")
                        .WithMany()
                        .HasForeignKey("OperatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Group", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Account", "Account")
                        .WithMany("Groups")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Operator", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Account", "Account")
                        .WithMany("Operators")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Transporter", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Device", "Device")
                        .WithOne("Transporter")
                        .HasForeignKey("TrackHub.Manager.Infrastructure.Entities.Transporter", "DeviceId");

                    b.Navigation("Device");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.User", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Account", "Account")
                        .WithMany("Users")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.UserGroup", b =>
                {
                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackHub.Manager.Infrastructure.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Account", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Operators");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Device", b =>
                {
                    b.Navigation("Transporter");
                });

            modelBuilder.Entity("TrackHub.Manager.Infrastructure.Entities.Operator", b =>
                {
                    b.Navigation("Credential");
                });
#pragma warning restore 612, 618
        }
    }
}
