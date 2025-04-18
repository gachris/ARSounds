﻿// <auto-generated />
using System;
using ARSounds.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ARSounds.EntityFramework.MySql.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ARSounds.EntityFramework.Entities.AudioAsset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("Audio")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AudioAssets");
                });

            modelBuilder.Entity("ARSounds.EntityFramework.Entities.ImageAsset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AudioAssetId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetime");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<bool>("IsTrackable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("OpenVisionId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("AudioAssetId")
                        .IsUnique();

                    b.ToTable("ImageAssets");
                });

            modelBuilder.Entity("ARSounds.EntityFramework.Entities.ImageAsset", b =>
                {
                    b.HasOne("ARSounds.EntityFramework.Entities.AudioAsset", "AudioAsset")
                        .WithOne("ImageAsset")
                        .HasForeignKey("ARSounds.EntityFramework.Entities.ImageAsset", "AudioAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AudioAsset");
                });

            modelBuilder.Entity("ARSounds.EntityFramework.Entities.AudioAsset", b =>
                {
                    b.Navigation("ImageAsset");
                });
#pragma warning restore 612, 618
        }
    }
}
