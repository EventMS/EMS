﻿// <auto-generated />
using System;
using EMS.Payment_Services.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EMS.Payment_Services.API.Migrations
{
    [DbContext(typeof(PaymentContext))]
    [Migration("20201028141451_RemoveDuplicateId")]
    partial class RemoveDuplicateId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EMS.Payment_Services.API.Context.Model.ClubSubscription", b =>
                {
                    b.Property<Guid>("ClubSubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StripePriceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StripeProductId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClubSubscriptionId");

                    b.ToTable("ClubSubscription");
                });

            modelBuilder.Entity("EMS.Payment_Services.API.Context.Model.Event", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float?>("PublicPrice")
                        .HasColumnType("real");

                    b.HasKey("EventId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("EMS.Payment_Services.API.Context.Model.EventPrice", b =>
                {
                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubSubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("EventId", "ClubSubscriptionId");

                    b.ToTable("EventPrice");
                });

            modelBuilder.Entity("EMS.Payment_Services.API.Context.Model.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StripeUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("EMS.Payment_Services.API.Context.Model.EventPrice", b =>
                {
                    b.HasOne("EMS.Payment_Services.API.Context.Model.Event", "Event")
                        .WithMany("EventPrices")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}