﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(WhatChoresDbContext))]
    [Migration("20240902003522_CharacterIdentification")]
    partial class CharacterIdentification
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccess.Tables.tbl_Characters", b =>
                {
                    b.Property<Guid>("CharacterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CharacterData")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Realm")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CharacterId");

                    b.ToTable("tbl_Characters");
                });

            modelBuilder.Entity("DataAccess.Tables.tbl_ClassData", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassId"));

                    b.Property<string>("ClassColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClassId");

                    b.ToTable("tbl_ClassData");
                });

            modelBuilder.Entity("DataAccess.Tables.tbl_MythicPlusValues", b =>
                {
                    b.Property<int>("KeyLevel")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KeyLevel"));

                    b.Property<int>("ItemLevel")
                        .HasColumnType("int");

                    b.HasKey("KeyLevel");

                    b.ToTable("tbl_MythicPlusValues");
                });

            modelBuilder.Entity("DataAccess.Tables.tbl_USRealms", b =>
                {
                    b.Property<int>("RealmID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RealmID"));

                    b.Property<string>("RealmName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RealmID");

                    b.ToTable("tbl_USRealms");
                });

            modelBuilder.Entity("DataAccess.Tables.tbl_VaultRaidBosses", b =>
                {
                    b.Property<string>("Boss")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Boss");

                    b.ToTable("tbl_VaultRaidBosses");
                });
#pragma warning restore 612, 618
        }
    }
}
