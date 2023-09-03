﻿// <auto-generated />
using System;
using CitiesManager.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CitiesManager.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230903070537_validation")]
    partial class validation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CitiesManager.WebApi.Model.Cities", b =>
                {
                    b.Property<Guid>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CityId");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            CityId = new Guid("f2bf0f42-53d3-4d82-be17-d4affc947cd8"),
                            CityName = "London"
                        },
                        new
                        {
                            CityId = new Guid("7e7adf8c-373c-4b09-9bfa-702a2f7b5364"),
                            CityName = "New York"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
