﻿// <auto-generated />
using System;
using AgedPoseDatabse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AgedPoseDatabse.Migrations
{
    [DbContext(typeof(AiForAgedDbContext))]
    [Migration("20230313153000_updateTable_newEvent")]
    partial class updateTable_newEvent
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AgedPoseDatabse.Models.AgesInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Address")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("Date");

                    b.Property<string>("ContacterName")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("ContacterPhone")
                        .HasColumnType("varchar(11)")
                        .HasMaxLength(11);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("NurseName")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<long>("RoomInfoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RoomInfoId");

                    b.ToTable("AgesInfo");
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.CameraInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("FactoryInfo")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("IpAddress")
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<bool>("IsUseSafeRegion")
                        .HasColumnType("bit");

                    b.Property<int>("LeftTopPointX")
                        .HasColumnType("int");

                    b.Property<int>("LeftTopPointY")
                        .HasColumnType("int");

                    b.Property<int>("RightBottomPointX")
                        .HasColumnType("int");

                    b.Property<int>("RightBottomPointY")
                        .HasColumnType("int");

                    b.Property<long>("RoomInfoId")
                        .HasColumnType("bigint");

                    b.Property<long>("ServerInfoId")
                        .HasColumnType("bigint");

                    b.Property<string>("VideoAddress")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("RoomInfoId");

                    b.HasIndex("ServerInfoId");

                    b.ToTable("CameraInfo");
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.DetailPoseInfo", b =>
                {
                    b.Property<long>("AgesInfoId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime");

                    b.Property<byte?>("Status")
                        .HasColumnType("tinyint");

                    b.HasKey("AgesInfoId", "DateTime");

                    b.ToTable("DetailPoseInfo");
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.PoseInfo", b =>
                {
                    b.Property<long>("AgesInfoId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<bool>("IsAlarm")
                        .HasColumnType("bit");

                    b.Property<byte?>("Status")
                        .HasColumnType("tinyint");

                    b.Property<int>("TimeDown")
                        .HasColumnType("int");

                    b.Property<string>("TimeIn")
                        .HasColumnType("varchar(8)")
                        .HasMaxLength(8);

                    b.Property<int>("TimeLie")
                        .HasColumnType("int");

                    b.Property<int>("TimeOther")
                        .HasColumnType("int");

                    b.Property<int>("TimeSit")
                        .HasColumnType("int");

                    b.Property<int>("TimeStand")
                        .HasColumnType("int");

                    b.HasKey("AgesInfoId", "Date");

                    b.ToTable("PoseInfo");
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.RecEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Img")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("RecEvent");
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.RoomInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<bool>("IsAlarm")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("RoomSize")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RoomInfo");
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.ServerInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("FactoryInfo")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<byte>("MaxCameraCount")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("Ip")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ServerInfo");
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Authority")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Password")
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.AgesInfo", b =>
                {
                    b.HasOne("AgedPoseDatabse.Models.RoomInfo", "RoomInfo")
                        .WithMany("AgesInfos")
                        .HasForeignKey("RoomInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.CameraInfo", b =>
                {
                    b.HasOne("AgedPoseDatabse.Models.RoomInfo", "RoomInfo")
                        .WithMany("CameraInfos")
                        .HasForeignKey("RoomInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AgedPoseDatabse.Models.ServerInfo", "ServerInfo")
                        .WithMany("CameraInfos")
                        .HasForeignKey("ServerInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.DetailPoseInfo", b =>
                {
                    b.HasOne("AgedPoseDatabse.Models.AgesInfo", "AgesInfo")
                        .WithMany()
                        .HasForeignKey("AgesInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AgedPoseDatabse.Models.PoseInfo", b =>
                {
                    b.HasOne("AgedPoseDatabse.Models.AgesInfo", "AgesInfo")
                        .WithMany("PoseInfoes")
                        .HasForeignKey("AgesInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
