﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CustomerServiceCampaign.Api.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20240131141847_CustomerTableInitial")]
    partial class CustomerTableInitial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Entities.Agent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("AgentId");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Ssn")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HomeCity")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("HomeState")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("HomeStreet")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("HomeZip")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OfficeCity")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OfficeState")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OfficeStreet")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OfficeZip")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SSN")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Salary")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });
#pragma warning restore 612, 618
        }
    }
}