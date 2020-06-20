using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Services.Common
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class HashUtility
    {
        public static string CreatePasswordHash(string password, string passwordSalt)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(passwordSalt)))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public static bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(passwordSalt)))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var oldHash = Convert.FromBase64String(passwordHash);

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != oldHash[i]) return false;
                }
                return true;
            }
        }
    }
}
