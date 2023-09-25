﻿// <auto-generated />
using System;
using Data.TestContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(Data.TestContext.TestContext))]
    partial class TestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            ExpiresAt = new DateTime(2023, 10, 11, 1, 31, 34, 443, DateTimeKind.Unspecified).AddTicks(3333),
                            IsRevoked = false,
                            IsUsed = false,
                            IssuedAt = new DateTime(2023, 9, 26, 1, 31, 34, 443, DateTimeKind.Unspecified).AddTicks(3333),
                            Token = "D66709F8-27E9-4164-AF91-CA0877F10FEF",
                            UserId = 1
                        },
                        new
                        {
                            Id = 3,
                            ExpiresAt = new DateTime(2023, 10, 11, 1, 31, 34, 443, DateTimeKind.Unspecified).AddTicks(3333),
                            IsRevoked = false,
                            IsUsed = false,
                            IssuedAt = new DateTime(2023, 9, 26, 1, 31, 34, 443, DateTimeKind.Unspecified).AddTicks(3333),
                            Token = "67C8F733-BF6A-4E49-8109-522DACCC60F7",
                            UserId = 2
                        });
                });

            modelBuilder.Entity("Entities.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("SecurityStamp")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Age = 30,
                            Email = "sample1@example.com",
                            FullName = "Sample User One",
                            IsActive = true,
                            PasswordHash = "8jr0nptvznD5VS2WniCx5y6jYyQOSw1ZpfsulA8c/3A=",
                            SecurityStamp = new Guid("0855b139-340a-47a5-9a24-a24b62886619"),
                            UserName = "ali"
                        },
                        new
                        {
                            Id = 2,
                            Age = 25,
                            Email = "sample2@example.com",
                            FullName = "Sample User Two",
                            IsActive = true,
                            PasswordHash = "8jr0nptvznD5VS2WniCx5y6jYyQOSw1ZpfsulA8c/3A=",
                            SecurityStamp = new Guid("68ff43f9-ff39-4b44-91fd-abd255efc354"),
                            UserName = "ata"
                        });
                });

            modelBuilder.Entity("Entities.Models.RefreshToken", b =>
                {
                    b.HasOne("Entities.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.Models.User", b =>
                {
                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
