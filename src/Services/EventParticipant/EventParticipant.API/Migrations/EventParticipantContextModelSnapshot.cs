﻿// <auto-generated />
using System;
using EMS.EventParticipant_Services.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EMS.EventParticipant_Services.API.Migrations
{
    [DbContext(typeof(EventParticipantContext))]
    partial class EventParticipantContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EMS.EventParticipant_Services.API.Context.Model.Event", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EventType")
                        .HasColumnType("int");

                    b.Property<bool?>("IsFree")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.HasKey("EventId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("EMS.EventParticipant_Services.API.Context.Model.EventParticipant", b =>
                {
                    b.Property<Guid>("EventParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EventId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EventParticipantId");

                    b.HasIndex("EventId");

                    b.HasIndex("EventId1");

                    b.ToTable("EventParticipant");
                });

            modelBuilder.Entity("EMS.EventParticipant_Services.API.Context.Model.EventPrice", b =>
                {
                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClubSubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EventId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("EventId", "ClubSubscriptionId");

                    b.HasIndex("EventId1");

                    b.ToTable("EventPrice");
                });

            modelBuilder.Entity("EMS.EventParticipant_Services.API.Context.Model.EventParticipant", b =>
                {
                    b.HasOne("EMS.EventParticipant_Services.API.Context.Model.Event", null)
                        .WithMany("EventParticipants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EMS.EventParticipant_Services.API.Context.Model.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId1");
                });

            modelBuilder.Entity("EMS.EventParticipant_Services.API.Context.Model.EventPrice", b =>
                {
                    b.HasOne("EMS.EventParticipant_Services.API.Context.Model.Event", null)
                        .WithMany("EventPrices")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EMS.EventParticipant_Services.API.Context.Model.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId1");
                });
#pragma warning restore 612, 618
        }
    }
}
