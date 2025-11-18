using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class UsersFavourite
{
    public int FavouriteId { get; set; }

    public int? FavouriteUserId { get; set; }

    public int? FavouriteBookId { get; set; }

    public virtual Book? FavouriteBook { get; set; }

    public virtual User? FavouriteUser { get; set; }
}
