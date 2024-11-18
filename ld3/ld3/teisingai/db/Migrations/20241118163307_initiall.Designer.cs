﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Org.Ktu.T120B197.DBs;

#nullable disable

namespace db.Migrations
{
    [DbContext(typeof(DB))]
    [Migration("20241118163307_initiall")]
    partial class initiall
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ExampleTag", b =>
                {
                    b.Property<int>("Examplesid")
                        .HasColumnType("int(11)");

                    b.Property<int>("Tagsid")
                        .HasColumnType("int(11)");

                    b.HasKey("Examplesid", "Tagsid");

                    b.HasIndex("Tagsid");

                    b.ToTable("ExampleTag");
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Author", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Category", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Comment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("exampleId")
                        .HasColumnType("int(11)");

                    b.HasKey("id");

                    b.HasIndex("exampleId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Example", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("Authorid")
                        .HasColumnType("int(11)");

                    b.Property<int?>("Categoryid")
                        .HasColumnType("int(11)");

                    b.Property<int>("number")
                        .HasColumnType("int(11)");

                    b.Property<string>("text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id")
                        .HasName("PRIMARY");

                    b.HasIndex("Authorid");

                    b.HasIndex("Categoryid");

                    b.ToTable("Example");
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Tag", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("ExampleTag", b =>
                {
                    b.HasOne("Org.Ktu.T120B197.DBs.Entities.Example", null)
                        .WithMany()
                        .HasForeignKey("Examplesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Org.Ktu.T120B197.DBs.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("Tagsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Comment", b =>
                {
                    b.HasOne("Org.Ktu.T120B197.DBs.Entities.Example", "Example")
                        .WithMany("Comments")
                        .HasForeignKey("exampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Example");
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Example", b =>
                {
                    b.HasOne("Org.Ktu.T120B197.DBs.Entities.Author", "Author")
                        .WithMany("Examples")
                        .HasForeignKey("Authorid");

                    b.HasOne("Org.Ktu.T120B197.DBs.Entities.Category", "Category")
                        .WithMany("Examples")
                        .HasForeignKey("Categoryid");

                    b.Navigation("Author");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Author", b =>
                {
                    b.Navigation("Examples");
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Category", b =>
                {
                    b.Navigation("Examples");
                });

            modelBuilder.Entity("Org.Ktu.T120B197.DBs.Entities.Example", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
