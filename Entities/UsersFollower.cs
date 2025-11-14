using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class UsersFollower
{
    public int FollowId { get; set; }

    public int? UserFollowId { get; set; }

    public int? UserFollowedId { get; set; }

    public virtual User? UserFollow { get; set; }

    public virtual User? UserFollowed { get; set; }
}
