﻿// <auto-generated />
using System;
using GrobsJobsRazorPages.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GrobsJobsRazorPages.Migrations.GrobsJobsRazorPagesDb
{
    [DbContext(typeof(GrobsJobsRazorPagesDbContext))]
    partial class GrobsJobsRazorPagesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("GrobsJobsRazorPages.Model.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTimePosted")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("JobPosterId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("JobPosterNormalizedUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("JobType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("GrobsJobsRazorPages.Model.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTimeSent")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageBody")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageRecipient")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageRecipientUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageSender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageSenderUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
