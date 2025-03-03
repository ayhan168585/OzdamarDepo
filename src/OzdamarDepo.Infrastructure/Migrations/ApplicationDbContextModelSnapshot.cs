﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OzdamarDepo.Infrastructure.Context;

#nullable disable

namespace OzdamarDepo.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OzdamarDepo.Domain.MediaItems.MediaItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ArtistOrDirector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("DiscCount")
                        .HasColumnType("int");

                    b.Property<bool?>("IsBoxSet")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("MediaItems");
                });

            modelBuilder.Entity("OzdamarDepo.Domain.MediaItems.MediaItem", b =>
                {
                    b.OwnsOne("OzdamarDepo.Domain.MediaItems.MediaCondition", "MediaCondition", b1 =>
                        {
                            b1.Property<Guid>("MediaItemId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("ConditionScore")
                                .HasColumnType("int")
                                .HasColumnName("ConditionScore");

                            b1.Property<string>("Description")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Description");

                            b1.HasKey("MediaItemId");

                            b1.ToTable("MediaItems");

                            b1.WithOwner()
                                .HasForeignKey("MediaItemId");
                        });

                    b.OwnsOne("OzdamarDepo.Domain.MediaItems.MediaType", "MediaType", b1 =>
                        {
                            b1.Property<Guid>("MediaItemId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Category")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Category");

                            b1.Property<string>("Format")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Format");

                            b1.HasKey("MediaItemId");

                            b1.ToTable("MediaItems");

                            b1.WithOwner()
                                .HasForeignKey("MediaItemId");
                        });

                    b.Navigation("MediaCondition")
                        .IsRequired();

                    b.Navigation("MediaType")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
