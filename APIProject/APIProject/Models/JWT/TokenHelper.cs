using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Models
{
    public static class TokenHelper
    {

        public static string GenerateSecureToken(int size = 64)
        {
            var bytes = RandomNumberGenerator.GetBytes(size);
            return Convert.ToBase64String(bytes); // opaque string
        }

        public static string Sha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes); // store hex
        }

    }
}
