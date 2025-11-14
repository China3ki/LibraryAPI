using System;
using System.Collections.Generic;

namespace LibraryAPI.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserSurname { get; set; }

    public string? UserNick { get; set; }

    public string? UserEmail { get; set; }

    public string? UserPassword { get; set; }

    public DateOnly? UserJoiningDate { get; set; }

    public int? UserType { get; set; }

    public string? UserImage { get; set; }

    public virtual ICollection<BooksLeased> BooksLeaseds { get; set; } = new List<BooksLeased>();

    public virtual ICollection<BooksReview> BooksReviews { get; set; } = new List<BooksReview>();

    public virtual UsersType? UserTypeNavigation { get; set; }

    public virtual ICollection<UsersFollower> UsersFollowerUserFolloweds { get; set; } = new List<UsersFollower>();

    public virtual ICollection<UsersFollower> UsersFollowerUserFollows { get; set; } = new List<UsersFollower>();

    public virtual ICollection<UsersReaded> UsersReadeds { get; set; } = new List<UsersReaded>();
}
