﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUNA.Models.BaseApplicationContext;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NUNA.Migrations
{
    [DbContext(typeof(BaseApplicationContext))]
    [Migration("20210928020647_AddMenuTop")]
    partial class AddMenuTop
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("uuid-ossp")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetRoleMenus", b =>
                {
                    b.Property<string>("RoleId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Menu")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("RoleId", "Menu");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleMenus");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetRolePermissions", b =>
                {
                    b.Property<string>("RoleId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Permission")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("RoleId", "Permission");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRolePermissions");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetRoles", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserLogins", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserMenus", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Menu")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("UserId", "Menu");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserMenus");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserPermissions", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Permission")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("UserId", "Permission");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserPermissions");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserRoles", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserTokens", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUsers", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique();

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_KabupatenKota", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("KabupatenKota")
                        .HasColumnType("text");

                    b.Property<int>("ProvinsiId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProvinsiId");

                    b.ToTable("ID_KabupatenKota");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_Kecamatan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("KabupatenKotaId")
                        .HasColumnType("integer");

                    b.Property<string>("Kecamatan")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("KabupatenKotaId");

                    b.ToTable("ID_Kecamatan");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_Kelurahan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("KecamatanId")
                        .HasColumnType("integer");

                    b.Property<string>("Kelurahan")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("KecamatanId");

                    b.ToTable("ID_Kelurahan");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_Provinsi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Provinsi")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ID_Provinsi");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.MS_Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Position")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("MS_Position");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ActionName")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Badge")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("BadgeStates")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Code")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("Controller")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("IconClass")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsParent")
                        .HasColumnType("boolean");

                    b.Property<string>("Nama")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("NoUrut")
                        .HasColumnType("integer");

                    b.Property<string>("Parent")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("Id");

                    b.ToTable("Menu");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.Officer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreationUser")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModificationUser")
                        .HasColumnType("text");

                    b.Property<int>("PersonalId")
                        .HasColumnType("integer");

                    b.Property<int?>("PositionId")
                        .HasColumnType("integer");

                    b.Property<byte[]>("Signature")
                        .HasMaxLength(75)
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.ToTable("Officer");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.Personals", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("BirthPlace")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreationUser")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModificationUser")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("UserName")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.ToTable("Personals");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.RF_IDType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Type")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.ToTable("RF_IDType");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.TR_Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("ContactInfo")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<int>("KabupatenKotaId")
                        .HasColumnType("integer");

                    b.Property<int>("KecamatanId")
                        .HasColumnType("integer");

                    b.Property<int>("KelurahanId")
                        .HasColumnType("integer");

                    b.Property<int>("PersonalId")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("PostCode")
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<int>("ProvinsiId")
                        .HasColumnType("integer");

                    b.Property<string>("Tag")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("Id");

                    b.HasIndex("KabupatenKotaId");

                    b.HasIndex("KecamatanId");

                    b.HasIndex("KelurahanId");

                    b.HasIndex("PersonalId");

                    b.HasIndex("ProvinsiId");

                    b.ToTable("TR_Address");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.TR_IDNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreationUser")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModificationUser")
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<int>("PersonalId")
                        .HasColumnType("integer");

                    b.Property<int?>("PersonalsId")
                        .HasColumnType("integer");

                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PersonalsId");

                    b.HasIndex("TypeId");

                    b.ToTable("TR_IDNumber");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.UserSetting", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Aside")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Email")
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)");

                    b.Property<string>("Theme")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("UserSetting");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetRoleClaims", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetRoles", "Role")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetRoleMenus", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetRoles", "Role")
                        .WithMany("AspNetRoleMenus")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetRolePermissions", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetRoles", "Role")
                        .WithMany("AspNetRolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserClaims", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetUsers", "User")
                        .WithMany("AspNetUserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserLogins", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetUsers", "User")
                        .WithMany("AspNetUserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserMenus", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetUsers", "User")
                        .WithMany("AspNetUserMenus")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserPermissions", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetUsers", "User")
                        .WithMany("AspNetUserPermissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserRoles", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetRoles", "Role")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetUsers", "User")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUserTokens", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.AspNetUsers", "User")
                        .WithMany("AspNetUserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_KabupatenKota", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.ID_Provinsi", "ID_Provinsi")
                        .WithMany("ID_KabupatenKota")
                        .HasForeignKey("ProvinsiId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ID_Provinsi");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_Kecamatan", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.ID_KabupatenKota", "ID_KabupatenKota")
                        .WithMany("ID_Kecamatan")
                        .HasForeignKey("KabupatenKotaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ID_KabupatenKota");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_Kelurahan", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.ID_Kecamatan", "ID_Kecamatan")
                        .WithMany("ID_Kelurahan")
                        .HasForeignKey("KecamatanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ID_Kecamatan");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.Officer", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.MS_Position", "MS_Position")
                        .WithMany("Officer")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("MS_Position");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.TR_Address", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.ID_KabupatenKota", "ID_KabupatenKota")
                        .WithMany("TR_Address")
                        .HasForeignKey("KabupatenKotaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NUNA.Models.BaseApplicationContext.ID_Kecamatan", "ID_Kecamatan")
                        .WithMany("TR_Address")
                        .HasForeignKey("KecamatanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NUNA.Models.BaseApplicationContext.ID_Kelurahan", "ID_Kelurahan")
                        .WithMany("TR_Address")
                        .HasForeignKey("KelurahanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NUNA.Models.BaseApplicationContext.Personals", "Personals")
                        .WithMany("TR_Address")
                        .HasForeignKey("PersonalId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NUNA.Models.BaseApplicationContext.ID_Provinsi", "ID_Provinsi")
                        .WithMany("TR_Address")
                        .HasForeignKey("ProvinsiId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ID_KabupatenKota");

                    b.Navigation("ID_Kecamatan");

                    b.Navigation("ID_Kelurahan");

                    b.Navigation("ID_Provinsi");

                    b.Navigation("Personals");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.TR_IDNumber", b =>
                {
                    b.HasOne("NUNA.Models.BaseApplicationContext.Personals", "Personals")
                        .WithMany("TR_IDNumber")
                        .HasForeignKey("PersonalsId");

                    b.HasOne("NUNA.Models.BaseApplicationContext.RF_IDType", "RF_IDType")
                        .WithMany("TR_IDNumber")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Personals");

                    b.Navigation("RF_IDType");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetRoles", b =>
                {
                    b.Navigation("AspNetRoleClaims");

                    b.Navigation("AspNetRoleMenus");

                    b.Navigation("AspNetRolePermissions");

                    b.Navigation("AspNetUserRoles");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.AspNetUsers", b =>
                {
                    b.Navigation("AspNetUserClaims");

                    b.Navigation("AspNetUserLogins");

                    b.Navigation("AspNetUserMenus");

                    b.Navigation("AspNetUserPermissions");

                    b.Navigation("AspNetUserRoles");

                    b.Navigation("AspNetUserTokens");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_KabupatenKota", b =>
                {
                    b.Navigation("ID_Kecamatan");

                    b.Navigation("TR_Address");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_Kecamatan", b =>
                {
                    b.Navigation("ID_Kelurahan");

                    b.Navigation("TR_Address");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_Kelurahan", b =>
                {
                    b.Navigation("TR_Address");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.ID_Provinsi", b =>
                {
                    b.Navigation("ID_KabupatenKota");

                    b.Navigation("TR_Address");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.MS_Position", b =>
                {
                    b.Navigation("Officer");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.Personals", b =>
                {
                    b.Navigation("TR_Address");

                    b.Navigation("TR_IDNumber");
                });

            modelBuilder.Entity("NUNA.Models.BaseApplicationContext.RF_IDType", b =>
                {
                    b.Navigation("TR_IDNumber");
                });
#pragma warning restore 612, 618
        }
    }
}
