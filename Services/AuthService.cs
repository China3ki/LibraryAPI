using LibraryAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace LibraryAPI.Services
{
    public class AuthService(LibraryContext ctx)
    {
        public bool VerifyPasswordEquality(string password, string confirmedPassword) => password == confirmedPassword;
        public bool VerifyPasswordRequirements(string password)
        {
            if (password.Length < 8) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!Regex.IsMatch(password, "[0-9]")) return false; // Numbers
            if (!Regex.IsMatch(password, "(?=.*?[#?!@$%^&*-])")) return false; // Special characters
            return true;
        }
        public async Task<bool> VerifyAccount(string email)
        {
            var account = await ctx.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
            if (account is null) return false;
            else return true;
        }
        public async Task<bool> VerifyEmail(string email)
        {
            if (email.Length == 0) return false;
            if (!Regex.IsMatch(email, "^\\S+@\\S+\\.\\S+$")) return false;
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
            if (user is null) return true;
            else return false;
        }
        public async Task<string> GetPasswordDb(string email)
        {
            var password = await ctx.Users.Select(u => u.UserPassword).FirstOrDefaultAsync();
            return password;
        }
    }
}
