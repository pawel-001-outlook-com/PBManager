using System.Security.Cryptography;
using System.Text;

namespace PBManager.Services.Helpers
{
    public class HashGenerator
    {
        public static string GenerateHash(string inputData)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(inputData));
                var builder = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }
    }
}