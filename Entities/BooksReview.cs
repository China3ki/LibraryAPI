using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class BooksReview
{
    public int ReviewId { get; set; }

    public int? ReviewBookId { get; set; }

    public int? ReviewUserId { get; set; }

    public string? ReviewText { get; set; }

    public int? ReviewRate { get; set; }

    public virtual Book? ReviewBook { get; set; }

    public virtual User? ReviewUser { get; set; }
}
