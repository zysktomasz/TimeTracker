﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeTracker.Persistance;

namespace TimeTracker.Persistance.Migrations
{
    [DbContext(typeof(TimeTrackerDbContext))]
    partial class TimeTrackerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TimeTracker.Domain.Entities.Activity", b =>
                {
                    b.Property<int>("ActivityID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("ProjectID");

                    b.Property<DateTime?>("TimeEnd");

                    b.Property<DateTime>("TimeStart");

                    b.Property<int?>("TimeTotal");

                    b.HasKey("ActivityID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("TimeTracker.Domain.Entities.Project", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("ProjectID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TimeTracker.Domain.Entities.Activity", b =>
                {
                    b.HasOne("TimeTracker.Domain.Entities.Project", "Project")
                        .WithMany("Activities")
                        .HasForeignKey("ProjectID");
                });
#pragma warning restore 612, 618
        }
    }
}
