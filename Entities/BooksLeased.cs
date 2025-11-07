using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class BooksLeased
{
    public int LeaseId { get; set; }

    public int? LeaseUserId { get; set; }

    public int? LeaseBookId { get; set; }

    public DateOnly? LeaseStartDate { get; set; }

    public DateOnly? LeaseEndDate { get; set; }

    public virtual Book? LeaseBook { get; set; }

    public virtual User? LeaseUser { get; set; }
}
