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
    [Migration("20240131152310_CustomerDateImported")]
    partial class CustomerDateImported
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

                    b.Property<DateTime>("DateImported")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HomeCity")
                        .HasColumnType("longtext");

                    b.Property<string>("HomeState")
                        .HasColumnType("longtext");

                    b.Property<string>("HomeStreet")
                        .HasColumnType("longtext");

                    b.Property<string>("HomeZip")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("OfficeCity")
                        .HasColumnType("longtext");

                    b.Property<string>("OfficeState")
                        .HasColumnType("longtext");

                    b.Property<string>("OfficeStreet")
                        .HasColumnType("longtext");

                    b.Property<string>("OfficeZip")
                        .HasColumnType("longtext");

                    b.Property<string>("SSN")
                        .HasColumnType("longtext");

                    b.Property<string>("Salary")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });
#pragma warning restore 612, 618
        }
    }
}
