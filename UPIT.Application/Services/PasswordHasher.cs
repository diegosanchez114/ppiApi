using Sodium;

namespace UPIT.Application.Services
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Calculate hash password using Argon2
            var hashedPassword = PasswordHash.ArgonHashString(password);
            return hashedPassword;            
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            // Chek if password intered is matches the stored hash
            return PasswordHash.ArgonHashStringVerify(hashedPassword, password);            
        }

        public static bool IsBase64String(string input)
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
