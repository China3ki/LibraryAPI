using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class BooksImage
{
    public int ImageId { get; set; }

    public int? ImageBookId { get; set; }

    public string? ImagePath { get; set; }

    public virtual Book? ImageBook { get; set; }
}
