using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class UsersType
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
