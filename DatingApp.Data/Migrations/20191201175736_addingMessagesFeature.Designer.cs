﻿// <auto-generated />
using System;
using DatingApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatingApp.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20191201175736_addingMessagesFeature")]
    partial class addingMessagesFeature
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DatingApp.Domain.Models.Like", b =>
                {
                    b.Property<int>("LikerId");

                    b.Property<int>("LikeeId");

                    b.HasKey("LikerId", "LikeeId");

                    b.HasIndex("LikeeId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("DatingApp.Domain.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime?>("DateReadAt");

                    b.Property<bool>("IsRead");

                    b.Property<DateTime>("MessageSentAt");

                    b.Property<bool>("RecipientDeleted");

                    b.Property<int>("RecipientId");

                    b.Property<bool>("SenderDeleted");

                    b.Property<int>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("DatingApp.Domain.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<bool>("IsMain");

                    b.Property<string>("PublicId");

                    b.Property<string>("Url");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("DatingApp.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Gender");

                    b.Property<string>("Interests");

                    b.Property<string>("Introduction");

                    b.Property<string>("KnownAs");

                    b.Property<DateTime>("LastActive");

                    b.Property<string>("LookingFor");

                    b.Property<byte[]>("PassswordSalt");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DatingApp.Domain.Models.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("DatingApp.Domain.Models.Like", b =>
                {
                    b.HasOne("DatingApp.Domain.Models.User", "Likee")
                        .WithMany("Likers")
                        .HasForeignKey("LikeeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DatingApp.Domain.Models.User", "Liker")
                        .WithMany("Likees")
                        .HasForeignKey("LikerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DatingApp.Domain.Models.Message", b =>
                {
                    b.HasOne("DatingApp.Domain.Models.User", "Recipient")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DatingApp.Domain.Models.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DatingApp.Domain.Models.Photo", b =>
                {
                    b.HasOne("DatingApp.Domain.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
