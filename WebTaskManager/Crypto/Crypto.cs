using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;

namespace WebTaskManager.Crypto
{
    public class Crypto
    {

        static HMACSHA512 SHA512 = new HMACSHA512(Encoding.Default.GetBytes("123"));
        public static string GetHash(string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            var hashBytes = SHA512.ComputeHash(bytes);
            return hashBytes.Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
        }

        public static string GenerateRandomSalt()
        {
            var ran = new Random();
            const int saltLen = 30;
            string res = String.Empty;
            for (int i = 0; i < saltLen; i++)
            {
                res += ran.Next(9).ToString();
            }
            return res;
        }

    }
}