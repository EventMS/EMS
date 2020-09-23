﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Permission.API.Context;

namespace Permission.API.Migrations
{
    [DbContext(typeof(PermissionContext))]
    partial class PermissionContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Permission.API.Context.Model.ClubAdministratorPermission", b =>
                {
                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClubId");

                    b.ToTable("ClubAdministratorPermission");
                });

            modelBuilder.Entity("Permission.API.Context.Model.UserAdministratorPermission", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClubId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "ClubId");

                    b.HasIndex("ClubId");

                    b.ToTable("UserAdministratorPermission");
                });

            modelBuilder.Entity("Permission.API.Context.Model.UserPermission", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId");

                    b.ToTable("UserPermission");
                });

            modelBuilder.Entity("Permission.API.Context.Model.UserAdministratorPermission", b =>
                {
                    b.HasOne("Permission.API.Context.Model.ClubAdministratorPermission", "ClubAdministratorPermission")
                        .WithMany("Users")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Permission.API.Context.Model.UserPermission", "UserPermission")
                        .WithMany("ClubAdminIn")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}