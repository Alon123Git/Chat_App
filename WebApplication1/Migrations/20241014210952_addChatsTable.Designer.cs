﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SERVER_SIDE.DBContext;

#nullable disable

namespace SERVER_SIDE.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20241014210952_addChatsTable")]
    partial class addChatsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SERVER_SIDE.Models.Chats", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id"));

                    b.Property<int>("_memberBelong_id")
                        .HasColumnType("int");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("_id");

                    b.HasIndex("_memberBelong_id");

                    b.ToTable("chatEntity");
                });

            modelBuilder.Entity("SERVER_SIDE.Models.Member", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id"));

                    b.Property<int>("_age")
                        .HasColumnType("int");

                    b.Property<string>("_gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("_isLogin")
                        .HasColumnType("bit");

                    b.Property<bool>("_isManager")
                        .HasColumnType("bit");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("_id");

                    b.ToTable("memberEntity");
                });

            modelBuilder.Entity("SERVER_SIDE.Models.Message", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id"));

                    b.Property<string>("_content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_sender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("_id");

                    b.ToTable("messageEntity");
                });

            modelBuilder.Entity("SERVER_SIDE.Models.Chats", b =>
                {
                    b.HasOne("SERVER_SIDE.Models.Member", "_memberBelong")
                        .WithMany()
                        .HasForeignKey("_memberBelong_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("_memberBelong");
                });
#pragma warning restore 612, 618
        }
    }
}
