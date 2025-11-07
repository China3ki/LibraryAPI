using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class UsersReaded
{
    public int ReadId { get; set; }

    public int? ReadUserId { get; set; }

    public int? ReadBookId { get; set; }

    public virtual Book? ReadBook { get; set; }

    public virtual User? ReadUser { get; set; }
}
