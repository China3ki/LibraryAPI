using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace LibraryAPI.Entities;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BooksLeased> BooksLeaseds { get; set; }

    public virtual DbSet<BooksReview> BooksReviews { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersReaded> UsersReadeds { get; set; }

    public virtual DbSet<UsersType> UsersTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;uid=user;database=library", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_polish_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PRIMARY");

            entity.ToTable("authors");

            entity.Property(e => e.AuthorId)
                .HasColumnType("int(11)")
                .HasColumnName("author_id");
            entity.Property(e => e.AuthorImage)
                .HasColumnType("text")
                .HasColumnName("author_image");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(50)
                .HasColumnName("author_name");
            entity.Property(e => e.AuthorSurname)
                .HasMaxLength(50)
                .HasColumnName("author_surname");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PRIMARY");

            entity.ToTable("books");

            entity.HasIndex(e => e.BookAuthorId, "fk_book_author_id");

            entity.HasIndex(e => e.BookCategoryId, "fk_book_category_id");

            entity.Property(e => e.BookId)
                .HasColumnType("int(11)")
                .HasColumnName("book_id");
            entity.Property(e => e.BookAmount)
                .HasColumnType("int(11)")
                .HasColumnName("book_amount");
            entity.Property(e => e.BookAuthorId)
                .HasColumnType("int(11)")
                .HasColumnName("book_author_id");
            entity.Property(e => e.BookCategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("book_category_id");
            entity.Property(e => e.BookCover)
                .HasColumnType("text")
                .HasColumnName("book_cover");
            entity.Property(e => e.BookDescription)
                .HasColumnType("text")
                .HasColumnName("book_description");
            entity.Property(e => e.BookReleaseDate)
                .HasColumnType("year(4)")
                .HasColumnName("book_releaseDate");
            entity.Property(e => e.BookTitle)
                .HasMaxLength(50)
                .HasColumnName("book_title");

            entity.HasOne(d => d.BookAuthor).WithMany(p => p.Books)
                .HasForeignKey(d => d.BookAuthorId)
                .HasConstraintName("fk_book_author_id");

            entity.HasOne(d => d.BookCategory).WithMany(p => p.Books)
                .HasForeignKey(d => d.BookCategoryId)
                .HasConstraintName("fk_book_category_id");
        });

        modelBuilder.Entity<BooksLeased>(entity =>
        {
            entity.HasKey(e => e.LeaseId).HasName("PRIMARY");

            entity.ToTable("books_leased");

            entity.HasIndex(e => e.LeaseBookId, "fk_lease_book_id");

            entity.HasIndex(e => e.LeaseUserId, "fk_lease_user_id");

            entity.Property(e => e.LeaseId)
                .HasColumnType("int(11)")
                .HasColumnName("lease_id");
            entity.Property(e => e.LeaseBookId)
                .HasColumnType("int(11)")
                .HasColumnName("lease_book_id");
            entity.Property(e => e.LeaseEndDate).HasColumnName("lease_endDate");
            entity.Property(e => e.LeaseStartDate).HasColumnName("lease_startDate");
            entity.Property(e => e.LeaseUserId)
                .HasColumnType("int(11)")
                .HasColumnName("lease_user_id");

            entity.HasOne(d => d.LeaseBook).WithMany(p => p.BooksLeaseds)
                .HasForeignKey(d => d.LeaseBookId)
                .HasConstraintName("fk_lease_book_id");

            entity.HasOne(d => d.LeaseUser).WithMany(p => p.BooksLeaseds)
                .HasForeignKey(d => d.LeaseUserId)
                .HasConstraintName("fk_lease_user_id");
        });

        modelBuilder.Entity<BooksReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.ToTable("books_reviews");

            entity.HasIndex(e => e.ReviewBookId, "fk_review_book_id");

            entity.HasIndex(e => e.ReviewUserId, "fk_review_user_id");

            entity.Property(e => e.ReviewId)
                .HasColumnType("int(11)")
                .HasColumnName("review_id");
            entity.Property(e => e.ReviewBookId)
                .HasColumnType("int(11)")
                .HasColumnName("review_book_id");
            entity.Property(e => e.ReviewRate)
                .HasColumnType("int(11)")
                .HasColumnName("review_rate");
            entity.Property(e => e.ReviewText)
                .HasMaxLength(300)
                .HasColumnName("review_text");
            entity.Property(e => e.ReviewUserId)
                .HasColumnType("int(11)")
                .HasColumnName("review_user_id");

            entity.HasOne(d => d.ReviewBook).WithMany(p => p.BooksReviews)
                .HasForeignKey(d => d.ReviewBookId)
                .HasConstraintName("fk_review_book_id");

            entity.HasOne(d => d.ReviewUser).WithMany(p => p.BooksReviews)
                .HasForeignKey(d => d.ReviewUserId)
                .HasConstraintName("fk_review_user_id");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.UserType, "fk_user_type");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(50)
                .HasColumnName("user_email");
            entity.Property(e => e.UserImage)
                .HasColumnType("text")
                .HasColumnName("user_image");
            entity.Property(e => e.UserJoiningDate).HasColumnName("user_joiningDate");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");
            entity.Property(e => e.UserNick)
                .HasMaxLength(50)
                .HasColumnName("user_nick");
            entity.Property(e => e.UserPassword)
                .HasColumnType("text")
                .HasColumnName("user_password");
            entity.Property(e => e.UserSurname)
                .HasMaxLength(50)
                .HasColumnName("user_surname");
            entity.Property(e => e.UserType)
                .HasColumnType("int(11)")
                .HasColumnName("user_type");

            entity.HasOne(d => d.UserTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserType)
                .HasConstraintName("fk_user_type");
        });

        modelBuilder.Entity<UsersReaded>(entity =>
        {
            entity.HasKey(e => e.ReadId).HasName("PRIMARY");

            entity.ToTable("users_readed");

            entity.HasIndex(e => e.ReadBookId, "fk_read_book_id");

            entity.HasIndex(e => e.ReadUserId, "fk_read_user_id");

            entity.Property(e => e.ReadId)
                .HasColumnType("int(11)")
                .HasColumnName("read_id");
            entity.Property(e => e.ReadBookId)
                .HasColumnType("int(11)")
                .HasColumnName("read_book_id");
            entity.Property(e => e.ReadUserId)
                .HasColumnType("int(11)")
                .HasColumnName("read_user_id");

            entity.HasOne(d => d.ReadBook).WithMany(p => p.UsersReadeds)
                .HasForeignKey(d => d.ReadBookId)
                .HasConstraintName("fk_read_book_id");

            entity.HasOne(d => d.ReadUser).WithMany(p => p.UsersReadeds)
                .HasForeignKey(d => d.ReadUserId)
                .HasConstraintName("fk_read_user_id");
        });

        modelBuilder.Entity<UsersType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PRIMARY");

            entity.ToTable("users_type");

            entity.Property(e => e.TypeId)
                .HasColumnType("int(11)")
                .HasColumnName("type_id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(20)
                .HasColumnName("type_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
