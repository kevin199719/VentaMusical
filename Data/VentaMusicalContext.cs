using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VentaMusical.Models.Entities;

namespace VentaMusical.Data;

public partial class VentaMusicalContext : DbContext
{
    public VentaMusicalContext()
    {
    }

    public VentaMusicalContext(DbContextOptions<VentaMusicalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Albume> Albumes { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardType> CardTypes { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<InvoiceHeader> InvoiceHeaders { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPassword> UserPasswords { get; set; }

    public virtual DbSet<UsersCard> UsersCards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("VentaMusicalContext");
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Albume>(entity =>
        {
            entity.ToTable("Albume");

            entity.Property(e => e.AlbumeId).ValueGeneratedNever();
            entity.Property(e => e.AlbumeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AlbumeYear).HasColumnType("date");

            entity.HasOne(d => d.Author).WithMany(p => p.Albumes)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Albume_Author");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Author");

            entity.Property(e => e.AuthorName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.ToTable("Card");

            entity.Property(e => e.CardCvv).HasColumnName("CardCVV");
            entity.Property(e => e.CardExpiration).HasColumnType("date");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .IsUnicode(false);

            entity.HasOne(d => d.CardTypeNavigation).WithMany(p => p.Cards)
                .HasForeignKey(d => d.CardType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Card_CardType");
        });

        modelBuilder.Entity<CardType>(entity =>
        {
            entity.ToTable("CardType");

            entity.Property(e => e.CardName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.ToTable("Gender");

            entity.Property(e => e.GenderDescription)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.ToTable("InvoiceDetail");

            entity.HasOne(d => d.InvoiceHeader).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetail_InvoiceHeader");

            entity.HasOne(d => d.Song).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetail_Song");
        });

        modelBuilder.Entity<InvoiceHeader>(entity =>
        {
            entity.ToTable("InvoiceHeader");

            entity.Property(e => e.InvoiceHeaderId).ValueGeneratedNever();
            entity.Property(e => e.InvoiceDate).HasColumnType("date");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.Property(e => e.ModuleName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasOne(d => d.Module).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Profiles_Modules");

            entity.HasOne(d => d.User).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Profiles_Users");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.ToTable("Song");

            entity.Property(e => e.SongName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SongPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SongYear).HasColumnType("date");

            entity.HasOne(d => d.Albume).WithMany(p => p.Songs)
                .HasForeignKey(d => d.AlbumeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Song_Albume");

            entity.HasOne(d => d.Author).WithMany(p => p.Songs)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Song_Author");

            entity.HasOne(d => d.Gender).WithMany(p => p.Songs)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Song_Gender");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserAlias)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserEmail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserIdentification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserPassword>(entity =>
        {
            entity.HasKey(e => e.PasswordId);

            entity.ToTable("User_Password");

            entity.Property(e => e.Password)
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserPasswords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Password_Users");
        });

        modelBuilder.Entity<UsersCard>(entity =>
        {
            entity.HasKey(e => e.UsersCardsId);

            entity.ToTable("Users_Cards");

            entity.Property(e => e.UsersCardsId).HasColumnName("Users_Cards_Id");
            entity.Property(e => e.UsersCardsState).HasColumnName("Users_Cards_State");

            entity.HasOne(d => d.Card).WithMany(p => p.UsersCards)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Cards_Card");

            entity.HasOne(d => d.User).WithMany(p => p.UsersCards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Cards_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
