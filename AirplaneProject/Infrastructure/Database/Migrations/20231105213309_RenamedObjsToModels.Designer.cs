﻿// <auto-generated />
using System;
using AirplaneProject.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AirplaneProject.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231105213309_RenamedObjsToModels")]
    partial class RenamedObjsToModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AirplaneProject.Objects.FlightModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DepartureDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DepartureLocation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DestinationLocation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("SeatingCapacity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Flight");
                });

            modelBuilder.Entity("AirplaneProject.Objects.OrderModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FlightId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("AirplaneProject.Objects.PassengerModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PassportData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Passenger");
                });

            modelBuilder.Entity("AirplaneProject.Objects.SeatReserveModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("OrderModelId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PassengerId")
                        .HasColumnType("uuid");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderModelId");

                    b.HasIndex("PassengerId");

                    b.ToTable("SeatReserve");
                });

            modelBuilder.Entity("AirplaneProject.Objects.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsEmployee")
                        .HasColumnType("boolean");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("AirplaneProject.Objects.OrderModel", b =>
                {
                    b.HasOne("AirplaneProject.Objects.FlightModel", "Flight")
                        .WithMany("Orders")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirplaneProject.Objects.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flight");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AirplaneProject.Objects.PassengerModel", b =>
                {
                    b.HasOne("AirplaneProject.Objects.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AirplaneProject.Objects.SeatReserveModel", b =>
                {
                    b.HasOne("AirplaneProject.Objects.OrderModel", null)
                        .WithMany("SeatReserves")
                        .HasForeignKey("OrderModelId");

                    b.HasOne("AirplaneProject.Objects.PassengerModel", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Passenger");
                });

            modelBuilder.Entity("AirplaneProject.Objects.FlightModel", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("AirplaneProject.Objects.OrderModel", b =>
                {
                    b.Navigation("SeatReserves");
                });
#pragma warning restore 612, 618
        }
    }
}
