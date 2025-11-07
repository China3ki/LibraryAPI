using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? AuthorName { get; set; }

    public string? AuthorSurname { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
