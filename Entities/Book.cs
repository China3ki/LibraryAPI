using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class Book
{
    public int BookId { get; set; }

    public int? BookAuthorId { get; set; }

    public int? BookCategoryId { get; set; }

    public string? BookTitle { get; set; }

    public string? BookDescription { get; set; }

    public short? BookReleaseDate { get; set; }

    public int? BookAmount { get; set; }

    public string? BookCover { get; set; }

    public virtual Author? BookAuthor { get; set; }

    public virtual Category? BookCategory { get; set; }

    public virtual ICollection<BooksLeased> BooksLeaseds { get; set; } = new List<BooksLeased>();

    public virtual ICollection<BooksReview> BooksReviews { get; set; } = new List<BooksReview>();

    public virtual ICollection<UsersFavourite> UsersFavourites { get; set; } = new List<UsersFavourite>();

    public virtual ICollection<UsersReaded> UsersReadeds { get; set; } = new List<UsersReaded>();
}
