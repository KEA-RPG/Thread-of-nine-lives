﻿// <auto-generated />
using System;
using Infrastructure.Persistance.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(RelationalContext))]
    partial class RelationalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.AuditLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ChangedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OperationType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryKeyValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Attack")
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<int>("Defence")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.ToTable("Cards", null, t =>
                        {
                            t.HasTrigger("trg_Audit_Cards_Delete");

                            t.HasTrigger("trg_Audit_Cards_Insert");

                            t.HasTrigger("trg_Audit_Cards_Update");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeckId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeckId")
                        .HasDatabaseName("idx_deck_id");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("DeckId"), new[] { "Text", "CreatedAt", "UserId" });

                    b.HasIndex("UserId");

                    b.ToTable("Comments", null, t =>
                        {
                            t.HasTrigger("trg_Audit_Comments_Delete");

                            t.HasTrigger("trg_Audit_Comments_Insert");

                            t.HasTrigger("trg_Audit_Comments_Update");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.Entities.Deck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsPublic")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IsPublic")
                        .HasDatabaseName("idx_is_public");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("IsPublic"), new[] { "UserId", "Name" });

                    b.HasIndex("UserId")
                        .HasDatabaseName("idx_user_id");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("UserId"), new[] { "Name", "IsPublic" });

                    b.ToTable("Decks", null, t =>
                        {
                            t.HasTrigger("trg_Audit_Decks_Delete");

                            t.HasTrigger("trg_Audit_Decks_Insert");

                            t.HasTrigger("trg_Audit_Decks_Update");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.Entities.DeckCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("DeckId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("DeckId");

                    b.ToTable("DeckCards", null, t =>
                        {
                            t.HasTrigger("trg_Audit_DeckCards_Delete");

                            t.HasTrigger("trg_Audit_DeckCards_Insert");

                            t.HasTrigger("trg_Audit_DeckCards_Update");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.Entities.Enemy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Health")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Enemies", null, t =>
                        {
                            t.HasTrigger("trg_Audit_Enemies_Delete");

                            t.HasTrigger("trg_Audit_Enemies_Insert");

                            t.HasTrigger("trg_Audit_Enemies_Update");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.Entities.Fight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EnemyId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EnemyId");

                    b.HasIndex("UserId");

                    b.ToTable("Fights", null, t =>
                        {
                            t.HasTrigger("trg_Audit_Fights_Delete");

                            t.HasTrigger("trg_Audit_Fights_Insert");

                            t.HasTrigger("trg_Audit_Fights_Update");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.Entities.GameAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FightId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FightId");

                    b.ToTable("GameActions", null, t =>
                        {
                            t.HasTrigger("trg_Audit_GameActions_Delete");

                            t.HasTrigger("trg_Audit_GameActions_Insert");

                            t.HasTrigger("trg_Audit_GameActions_Update");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .HasDatabaseName("idx_username");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("Username"), new[] { "PasswordHash", "Role" });

                    b.ToTable("Users", null, t =>
                        {
                            t.HasTrigger("trg_Audit_Users_Delete");

                            t.HasTrigger("trg_Audit_Users_Insert");

                            t.HasTrigger("trg_Audit_Users_Update");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.HasOne("Domain.Entities.Deck", "Deck")
                        .WithMany("Comments")
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Deck");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Deck", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.DeckCard", b =>
                {
                    b.HasOne("Domain.Entities.Card", "Card")
                        .WithMany("DeckCards")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Deck", "Deck")
                        .WithMany("DeckCards")
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Deck");
                });

            modelBuilder.Entity("Domain.Entities.Fight", b =>
                {
                    b.HasOne("Domain.Entities.Enemy", "Enemy")
                        .WithMany("Fights")
                        .HasForeignKey("EnemyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Fights")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Enemy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.GameAction", b =>
                {
                    b.HasOne("Domain.Entities.Fight", "Fight")
                        .WithMany("GameActions")
                        .HasForeignKey("FightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fight");
                });

            modelBuilder.Entity("Domain.Entities.Card", b =>
                {
                    b.Navigation("DeckCards");
                });

            modelBuilder.Entity("Domain.Entities.Deck", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("DeckCards");
                });

            modelBuilder.Entity("Domain.Entities.Enemy", b =>
                {
                    b.Navigation("Fights");
                });

            modelBuilder.Entity("Domain.Entities.Fight", b =>
                {
                    b.Navigation("GameActions");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Fights");
                });
#pragma warning restore 612, 618
        }
    }
}
