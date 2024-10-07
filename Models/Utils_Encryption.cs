using System.Security.Cryptography;
using System.Text;

namespace Dynatherm_Eevee.Models
{
    public class Utils_Encryption
    {
        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000))
            {
                byte[] hash = deriveBytes.GetBytes(32); // 32 bytes for a 256-bit hash
                return Convert.ToBase64String(hash);
            }
        }
    }
}
