﻿// <auto-generated />
using System;
using EMS.ClubMember_Services.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EMS.ClubMember_Services.API.Migrations
{
    [DbContext(typeof(ClubMemberContext))]
    partial class ClubMemberContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EMS.ClubMember_Services.API.Context.Model.ClubMember", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubSubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "ClubId");

                    b.HasIndex("ClubSubscriptionId");

                    b.ToTable("ClubMember");
                });

            modelBuilder.Entity("EMS.ClubMember_Services.API.Context.Model.ClubSubscription", b =>
                {
                    b.Property<Guid>("ClubSubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClubSubscriptionId");

                    b.ToTable("ClubSubscription");
                });

            modelBuilder.Entity("EMS.ClubMember_Services.API.Context.Model.ClubMember", b =>
                {
                    b.HasOne("EMS.ClubMember_Services.API.Context.Model.ClubSubscription", null)
                        .WithMany("ClubMembers")
                        .HasForeignKey("ClubSubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
