﻿// <auto-generated />
using System;
using MemBotReal.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MemBotReal.Migrations.PostgresMigrations
{
    [DbContext(typeof(PostgresqlContext))]
    [Migration("20240401070702_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BotBase.Database.GuildPrefixPreference", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Prefix")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GuildPrefixPreferences");
                });

            modelBuilder.Entity("MemBotReal.Database.Models.Case", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("CaseType")
                        .HasColumnType("integer");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ModId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("OffenderId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GuildId", "OffenderId");

                    b.ToTable("Cases");
                });

            modelBuilder.Entity("MemBotReal.Database.Models.CustomCommand", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Contents")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("OwnerId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("GuildId", "Name");

                    b.ToTable("CustomCommands");
                });

            modelBuilder.Entity("MemBotReal.Database.Models.DelayedUnmute", b =>
                {
                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("OffenderId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset>("UnmuteWhen")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("GuildId", "OffenderId");

                    b.ToTable("DelayedUnmutes");
                });

            modelBuilder.Entity("MemBotReal.Database.Models.GuildConfig", b =>
                {
                    b.Property<decimal>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal[]>("CasesAllowedChannels")
                        .IsRequired()
                        .HasColumnType("numeric(20,0)[]");

                    b.Property<decimal>("LoggingChannel")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ModRole")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("MuteRole")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("GuildId");

                    b.ToTable("GuildConfigs");
                });
#pragma warning restore 612, 618
        }
    }
}
