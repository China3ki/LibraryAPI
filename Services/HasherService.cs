using System.Security.Cryptography;
using System.Text;

namespace LibraryAPI.Services
{
    public class HasherService
    {
        private const int _hash = 32;
        private const int _salt = 16;
        private const int _iterations = 100000;
        private readonly HashAlgorithmName _alghoritm = HashAlgorithmName.SHA512;
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_salt);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _alghoritm, _hash);

            return $"{Convert.ToHexString(hash)}.{Convert.ToHexString(salt)}";
        } 
        public bool VerifyPassword(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split(".");
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _alghoritm, _hash);

            return hash.SequenceEqual(inputHash);
        }
    }
}
