using System;
using System.Security.Cryptography;
using System.Text;

namespace LB3_DS_BLOCKSHAIN
{

    public class Hash
    {
        public static string ToSHA1(string data)
        {
            using (var sha1 = SHA1.Create())
            {
                var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToHexString(hashBytes);
            }
        }

        public static string ToSHA256(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToHexString(hashBytes);
            }
        }
    }
}